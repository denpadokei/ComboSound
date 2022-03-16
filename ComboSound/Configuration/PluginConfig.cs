using IPA.Config.Stores;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace ComboSound.Configuration
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual bool Enable { get; set; } = true;
        public virtual int Volume { get; set; } = 100;
        public virtual string CurrentSound { get; set; } = "";

        public event Action<PluginConfig> OnReloadEvent;
        public event Action<PluginConfig> OnChangeEvent;


        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
            this.OnReloadEvent?.Invoke(this);
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
            this.OnChangeEvent?.Invoke(this);
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
            this.Enable = other.Enable;
            this.Volume = other.Volume;
        }
    }
}