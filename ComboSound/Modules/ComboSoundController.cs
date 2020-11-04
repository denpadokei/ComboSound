using ComboSound.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
        [Inject]
        ScoreController _scoreController;

        private AudioSource[] _audioSources;

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            Logger.Debug($"{name}: Awake()");
        }
        

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Logger.Debug($"{name}: OnDestroy()");
            _scoreController.comboDidChangeEvent -= this.ScoreController_comboDidChangeEvent;
        }
        #endregion
        [Inject]
        void Constractor()
        {
            _scoreController.comboDidChangeEvent -= this.ScoreController_comboDidChangeEvent;
            _scoreController.comboDidChangeEvent += this.ScoreController_comboDidChangeEvent;
            
        }

        public void Initialize()
        {
            this.StartCoroutine(this.CreateAudioSources());
        }

        IEnumerator CreateAudioSources()
        {
            var audios = new List<AudioSource>();
            for (int i = 0; i < 12; i++) {
                var songPath = Path.Combine(Plugin.DataPath, $"combo_{i + 1:000}.wav");
                Logger.Debug(songPath);
                var song = UnityWebRequestMultimedia.GetAudioClip(songPath, AudioType.WAV);
                yield return song.SendWebRequest();
                if (song.error != null) {
                    Logger.Error($"{song.error}");
                }
                else {
                    var audio = this.gameObject.AddComponent<AudioSource>();
                    try {
                        audio.clip = DownloadHandlerAudioClip.GetContent(song);
                        audios.Add(audio);
                    }
                    catch (Exception e) {
                        Logger.Error(e);
                        continue;
                    }
                    yield return new WaitWhile(() => !audio.clip);
                }
                
            }
            this._audioSources = audios.ToArray();
        }

        private void ScoreController_comboDidChangeEvent(int obj)
        {
            if (this._audioSources == null) {
                return;
            }

            switch (obj) {
                case 50:
                    this._audioSources[0].Play();
                    break;
                case 100:
                    this._audioSources[1].Play();
                    break;
                case 200:
                    this._audioSources[2].Play();
                    break;
                case 300:
                    this._audioSources[3].Play();
                    break;
                case 400:
                    this._audioSources[4].Play();
                    break;
                case 500:
                    this._audioSources[5].Play();
                    break;
                case 600:
                    this._audioSources[6].Play();
                    break;
                case 700:
                    this._audioSources[7].Play();
                    break;
                case 800:
                    this._audioSources[8].Play();
                    break;
                case 900:
                    this._audioSources[9].Play();
                    break;
                case 1000:
                    this._audioSources[10].Play();
                    break;
                case 1100:
                    this._audioSources[11].Play();
                    break;
                default:
                    break;
            }
        }
    }
}
