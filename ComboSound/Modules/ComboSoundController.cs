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

        private Dictionary<int, AudioSource> _audioSources;

        private static readonly Regex _songFileRegex = new Regex(@"combo_[0-9]{4}\.wav");

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            Logger.Debug($"{name}: Awake()");
            if (!SettingView.instance.IsEnable) {
                Logger.Debug("Combo sound is Disable");
                return;
            }
            _scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().First();
            _scoreController.comboDidChangeEvent -= this.ScoreController_comboDidChangeEvent;
            _scoreController.comboDidChangeEvent += this.ScoreController_comboDidChangeEvent;
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
        public void Initialize()
        {
            this.StartCoroutine(this.CreateAudioSources());
        }

        IEnumerator CreateAudioSources()
        {
            var audios = new List<KeyValuePair<int, AudioSource>>();
            foreach (var songPath in Directory.EnumerateFiles(Plugin.DataPath, "*.wav", SearchOption.AllDirectories).Where(x => _songFileRegex.IsMatch(Path.GetFileName(x)))) {
                Logger.Debug(songPath);
                var song = UnityWebRequestMultimedia.GetAudioClip(songPath, AudioType.WAV);
                yield return song.SendWebRequest();
                if (!string.IsNullOrEmpty(song.error)) {
                    Logger.Error($"{song.error}");
                }
                else {
                    var audio = this.gameObject.AddComponent<AudioSource>();
                    try {
                        audio.clip = DownloadHandlerAudioClip.GetContent(song);
                        audio.clip.name = Path.GetFileName(songPath);
                        var conboNum = Regex.Match(audio.clip.name, "[0-9]{4}").Value;
                        if (int.TryParse(conboNum, out var number)) {
                            audios.Add(new KeyValuePair<int, AudioSource>(number, audio));
                        }
                    }
                    catch (Exception e) {
                        Logger.Error(e);
                        continue;
                    }
                    yield return new WaitWhile(() => !audio.clip);
                }
            }

            this._audioSources = new Dictionary<int, AudioSource>(audios);
        }

        private void ScoreController_comboDidChangeEvent(int obj)
        {
            if (this._audioSources != null && this._audioSources.TryGetValue(obj, out var audioSource)) {
                audioSource.Play();
            }
        }
    }
}
