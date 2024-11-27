using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using ComboSound.Configuration;
using ComboSound.Modules;
using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ComboSound.Views
{
    [HotReload]
    public class SettingView : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", this.GetType().Namespace, this.GetType().Name);

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

        [Inject]
        public void Constractor(SoundManager soundManager)
        {
            this._soundManager = soundManager;
        }

        public void Initialize()
        {
            GameplaySetup.Instance.AddTab("COMBO SOUND", this.ResourceName, this);
            if (string.IsNullOrEmpty(PluginConfig.Instance.CurrentSound))
            {
                var key = this._soundManager.Sounds.FirstOrDefault().Key;
                if (!string.IsNullOrEmpty(key)) {
                    PluginConfig.Instance.CurrentSound = Path.GetFileName(key);
                }
            }
        }

        private IEnumerator CreateList()
        {
            yield return new WaitWhile(() => this._soundManager.IsLoading);
            try
            {

                Logger.Debug($"sounds count : {this._soundManager.Sounds.Count}");

                var sounds = new List<string>();
                foreach (var item in this._soundManager.Sounds)
                {
                    sounds.Add(new DirectoryInfo(item.Key).Name);
                }
                if (this._dropDownObject is LayoutElement layout)
                {
                    Logger.Debug("Reload!");
                    this._sounds = sounds.OrderBy(x => x).ToList();
                    this._simpleTextDropdown = layout.GetComponentsInChildren<SimpleTextDropdown>(true).FirstOrDefault();
                    this._simpleTextDropdown.SetTexts(this._sounds);
                    this._simpleTextDropdown.ReloadData();
                    this._simpleTextDropdown.didSelectCellWithIdxEvent += this.OnSimpleTextDropdown_didSelectCellWithIdxEvent;
                    if (string.IsNullOrEmpty(PluginConfig.Instance.CurrentSound))
                    {
                        PluginConfig.Instance.CurrentSound = this._sounds.FirstOrDefault() ?? "";
                    }
                    else if (this._sounds.Any(x => x == PluginConfig.Instance.CurrentSound))
                    {
                        this._simpleTextDropdown.SelectCellWithIdx(this._sounds.IndexOf(PluginConfig.Instance.CurrentSound));
                    }
                    else
                    {
                        this._simpleTextDropdown.SelectCellWithIdx(0);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void OnSimpleTextDropdown_didSelectCellWithIdxEvent(DropdownWithTableView arg1, int arg2)
        {
            PluginConfig.Instance.CurrentSound = this._sounds[arg2];
            Logger.Debug(PluginConfig.Instance.CurrentSound);
        }

        [UIAction("#post-parse")]
        public void PostParse()
        {
            _ = this.StartCoroutine(this.CreateList());
        }

        [UIComponent("sound-dropdown")]
        private readonly object _dropDownObject;
        private SimpleTextDropdown _simpleTextDropdown;
        private SoundManager _soundManager;
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    try
                    {
                        if (this._simpleTextDropdown)
                        {
                            this._simpleTextDropdown.didSelectCellWithIdxEvent -= this.OnSimpleTextDropdown_didSelectCellWithIdxEvent;
                        }
                        GameplaySetup.Instance?.RemoveTab("COMBO SOUND");
                    }
                    catch (Exception)
                    {
                    }
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                this.disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~SettingView()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
