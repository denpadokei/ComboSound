using ComboSound.Configuration;
using SiraUtil.Zenject;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace ComboSound.Modules
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class ComboSoundController : MonoBehaviour, IAsyncInitializable
    {
        private IComboController _comboController;
        private Dictionary<int, AudioClip> _audioSources;
        private AudioSource _audioSource;



        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Logger.Debug($"{this.name}: OnDestroy()");
            if (this._comboController != null) {
                this._comboController.comboDidChangeEvent -= this.ScoreController_comboDidChangeEvent;
            }
        }
        #endregion
        [Inject]
        private void Constractor(IComboController comboController)
        {
            if (!PluginConfig.Instance.Enable) {
                Logger.Debug("Combo sound is Disable");
                return;
            }
            this._comboController = comboController;
            this._comboController.comboDidChangeEvent -= this.ScoreController_comboDidChangeEvent;
            this._comboController.comboDidChangeEvent += this.ScoreController_comboDidChangeEvent;
        }

        public async Task InitializeAsync(CancellationToken token)
        {
            this._audioSource = this.gameObject.AddComponent<AudioSource>();
            await this.CreateAudioSources(token);
        }

        private async Task CreateAudioSources(CancellationToken token)
        {
            while (!token.IsCancellationRequested && this._audioSource == null) {
                await Task.Yield();
            }
            if (token.IsCancellationRequested) {
                return;
            }
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
