using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using ComboSound.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ComboSound.Views
{
    public class SettingView : PersistentSingleton<SettingView>, INotifyPropertyChanged
    {
        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);
        
        
        /// <summary>説明 を取得、設定</summary>
        [UIValue("is-enable")]
        public bool IsEnable
        {
            get => PluginConfig.Instance.Enable;

            set
            {
                PluginConfig.Instance.Enable = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>説明 を取得、設定</summary>
        [UIValue("volume")]
        public int Volume
        {
            get => PluginConfig.Instance.Volume;

            set
            {
                PluginConfig.Instance.Volume = value;
                this.NotifyPropertyChanged();
            }
        }

        void Awake()
        {
            this.IsEnable = PluginConfig.Instance.Enable;
            this.Volume = PluginConfig.Instance.Volume;
        }

        #region INotifyPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged([CallerMemberName] string member = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(member));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
            Logger.Debug($"PropertyChange : {e.PropertyName}");
        }
        #endregion
    }
}
