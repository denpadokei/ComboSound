using ComboSound.Configuration;
using ComboSound.Utilities;
using ComboSound.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace ComboSound.Modules
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class ComboSoundController : MonoBehaviour, IInitializable
    {
        ScoreController _scoreController;
        private Dictionary<int, AudioClip> _audioSources;
        AudioSource _audioSource;

        

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            Logger.Debug($"{name}: Awake()");
            this._audioSource = this.gameObject.AddComponent<AudioSource>();
        }
        

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Logger.Debug($"{name}: OnDestroy()");
            if (this._scoreController != null) {
                this._scoreController.comboDidChangeEvent -= this.ScoreController_comboDidChangeEvent;
            }
        }
        #endregion
        [Inject]
        void Constractor(DiContainer container)
        {
            if (!PluginConfig.Instance.Enable) {
                Logger.Debug("Combo sound is Disable");
                return;
            }

            try {
                this._scoreController = container.Resolve<ScoreController>();
            }
            catch (Exception e) {
                Logger.Error(e);
                return;
            }

            this._scoreController.comboDidChangeEvent -= this.ScoreController_comboDidChangeEvent;
            this._scoreController.comboDidChangeEvent += this.ScoreController_comboDidChangeEvent;
        }

        public void Initialize()
        {
            this.CreateAudioSources();
        }

        void CreateAudioSources()
        {
            if (SoundManager.Sounds.TryGetValue(Path.Combine(Plugin.DataPath, PluginConfig.Instance.CurrentSound), out var dictionary)) {
                this._audioSource.volume = PluginConfig.Instance.Volume / 100f;
                this._audioSources = dictionary;
            }
        }

        private void ScoreController_comboDidChangeEvent(int obj)
        {
            if (this._audioSources != null && this._audioSources.TryGetValue(obj, out var audioClip)) {
                this._audioSource.PlayOneShot(audioClip);
            }
        }
    }
}
