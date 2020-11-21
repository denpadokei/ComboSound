using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using ComboSound.Configuration;
using ComboSound.Modules;
using HMUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [UIValue("sounds")]
        public List<object> Sounds { get; set; } = new List<object>() { "Don-Chan" };
        private List<string> _sounds;
        void Awake()
        {
            this.StartCoroutine(SoundManager.LoadSounds());
        }

        protected override void OnDestroy()
        {
            this._simpleTextDropdown.didSelectCellWithIdxEvent -= this._simpleTextDropdown_didSelectCellWithIdxEvent;
            base.OnDestroy();
        }

        private IEnumerator CreateList()
        {
            yield return new WaitWhile(() => SoundManager.IsLoading);
            try {
                
                Logger.Debug($"sounds count : {SoundManager.Sounds.Count}");

                var sounds = new List<string>();
                foreach (var item in SoundManager.Sounds) {
                    sounds.Add(new DirectoryInfo(item.Key).Name);
                }
                if (this._dropDownObject is LayoutElement layout) {
                    Logger.Debug("Reload!");
                    this._sounds = sounds.OrderBy(x => x).ToList();
                    this._simpleTextDropdown = layout.GetComponentsInChildren<SimpleTextDropdown>(true).FirstOrDefault();
                    this._simpleTextDropdown.SetTexts(this._sounds);
                    this._simpleTextDropdown.ReloadData();
                    this._simpleTextDropdown.didSelectCellWithIdxEvent += this._simpleTextDropdown_didSelectCellWithIdxEvent;
                    if (string.IsNullOrEmpty(PluginConfig.Instance.CurrentSound)) {
                        PluginConfig.Instance.CurrentSound = this._sounds.FirstOrDefault() ?? "";
                    }
                    else if (this._sounds.Any(x => x == PluginConfig.Instance.CurrentSound)) {
                        this._simpleTextDropdown.SelectCellWithIdx(this._sounds.IndexOf(PluginConfig.Instance.CurrentSound));
                    }
                    else {
                        this._simpleTextDropdown.SelectCellWithIdx(0);
                    }
                }
            }
            catch (Exception e) {
                Logger.Error(e);
            }
        }

        private void _simpleTextDropdown_didSelectCellWithIdxEvent(DropdownWithTableView arg1, int arg2)
        {
            PluginConfig.Instance.CurrentSound = this._sounds[arg2];
            Logger.Debug(PluginConfig.Instance.CurrentSound);
        }

        [UIAction("#post-parse")]
        void PostParse()
        {
            this.StartCoroutine(this.CreateList());
        }
        [UIComponent("sound-dropdown")]
        private object _dropDownObject;
        SimpleTextDropdown _simpleTextDropdown;


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
