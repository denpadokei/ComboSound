using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ComboSound.Modules
{
    public class SoundManager
    {
        public static ConcurrentDictionary<string, Dictionary<int, AudioClip>> Sounds { get; set; } = new ConcurrentDictionary<string, Dictionary<int, AudioClip>>();
        private static readonly Regex _songFileRegex = new Regex(@"combo_[0-9]{4}\.wav");
        public static volatile bool IsLoading = false;

        public static IEnumerator LoadSounds()
        {
            IsLoading = true;
            Logger.Debug("LoadSound call");
            Sounds.Clear();

            foreach (var songDirectory in Directory.EnumerateDirectories(Plugin.DataPath, "*", SearchOption.TopDirectoryOnly)) {
                var dictionary = new Dictionary<int, AudioClip>();
                foreach (var songPath in Directory.EnumerateFiles(songDirectory, "*.wav", SearchOption.TopDirectoryOnly).Where(x => _songFileRegex.IsMatch(Path.GetFileName(x)))) {
                    Logger.Debug(songPath);
                    var song = UnityWebRequestMultimedia.GetAudioClip(songPath, AudioType.WAV);
                    yield return song.SendWebRequest();
                    if (!string.IsNullOrEmpty(song.error)) {
                        Logger.Error($"{song.error}");
                    }
                    else {
                        var clip = DownloadHandlerAudioClip.GetContent(song);
                        try {
                            clip.name = Path.GetFileName(songPath);
                            var conboNum = Regex.Match(clip.name, "[0-9]{4}").Value;

                            if (int.TryParse(conboNum, out var number)) {
                                dictionary.TryAdd(number, clip);
                            }
                        }
                        catch (Exception e) {
                            Logger.Error(e);
                            continue;
                        }
                        yield return new WaitWhile(() => !clip);
                    }
                }
                Sounds.AddOrUpdate(songDirectory, dictionary, (s, d) => dictionary);
            }
            Logger.Debug("Finish LoadSong");
            IsLoading = false;
        }
    }
}
