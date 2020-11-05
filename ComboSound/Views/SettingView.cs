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
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ComboSound.Views
{
    public class SettingView : PersistentSingleton<SettingView>, INotifyPropertyChanged
    {
        // For this method of setting the ResourceName, this class must be the first class in the file.
        public string ResourceName => string.Join(".", GetType().Namespace, GetType().Name);
        
        /// <summary>説明 を取得、設定</summary>
        private bool isEnable_ = true;
        /// <summary>説明 を取得、設定</summary>
        [UIValue("is-enable")]
        public bool IsEnable
        {
            get => this.isEnable_;

            set
            {
                this.isEnable_ = value;
                this.NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void NotifyPropertyChanged([CallerMemberName] string member = null)
        {
            Logger.Debug($"PropertyChange : {member}");
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member));
        }
    }
}
