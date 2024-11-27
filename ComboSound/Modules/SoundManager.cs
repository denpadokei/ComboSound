using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace ComboSound.Modules
{
    public class SoundManager : MonoBehaviour, IInitializable
    {
        public ConcurrentDictionary<string, Dictionary<int, AudioClip>> Sounds { get; set; } = new ConcurrentDictionary<string, Dictionary<int, AudioClip>>();
        private static readonly Regex _songFileRegex = new Regex(@"combo_[0-9]{4}\.wav");
        public volatile bool IsLoading = false;

        public IEnumerator LoadSounds()
        {
            this.IsLoading = true;
            Logger.Debug("LoadSound call");
            this.Sounds.Clear();

            foreach (var songDirectory in Directory.EnumerateDirectories(Plugin.DataPath, "*", SearchOption.TopDirectoryOnly))
            {
                var dictionary = new Dictionary<int, AudioClip>();
                foreach (var songPath in Directory.EnumerateFiles(songDirectory, "*.wav", SearchOption.TopDirectoryOnly).Where(x => _songFileRegex.IsMatch(Path.GetFileName(x))))
                {
                    Logger.Debug(songPath);
                    var song = UnityWebRequestMultimedia.GetAudioClip(songPath, AudioType.WAV);
                    yield return song.SendWebRequest();
                    if (!string.IsNullOrEmpty(song.error))
                    {
                        Logger.Error($"{song.error}");
                    }
                    else
                    {
                        var clip = DownloadHandlerAudioClip.GetContent(song);
                        try
                        {
                            clip.name = Path.GetFileName(songPath);
                            var conboNum = Regex.Match(clip.name, "[0-9]{4}").Value;

                            if (int.TryParse(conboNum, out var number))
                            {
                                _ = dictionary.TryAdd(number, clip);
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e);
                            continue;
                        }
                        yield return new WaitWhile(() => !clip);
                    }
                }
                _ = this.Sounds.AddOrUpdate(songDirectory, dictionary, (s, d) => dictionary);
            }
            Logger.Debug("Finish LoadSong");
            this.IsLoading = false;
        }

        public void Initialize()
        {
            _ = this.StartCoroutine(this.LoadSounds());
        }
    }
}
