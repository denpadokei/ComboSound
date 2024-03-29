﻿using BeatSaberMarkupLanguage.GameplaySetup;
using ComboSound.Installer;
using ComboSound.Views;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using System;
using System.IO;
using IPALogger = IPA.Logging.Logger;

namespace ComboSound
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        public static string DataPath => Path.Combine(Environment.CurrentDirectory, "UserData", "ComboSound");
        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger, Config conf, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            Log.Info("ComboSound initialized.");
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
            zenjector.Install<ComboSoundInstaller>(Location.Player);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
        }

        [OnEnable]
        public void OnEnable()
        {
            try {
                Logger.Debug("OnEnable call");
                Logger.Debug(SettingView.instance.ResourceName);
                GameplaySetup.instance.AddTab("COMBO SOUND", SettingView.instance.ResourceName, SettingView.instance);
            }
            catch (Exception e) {
                Logger.Error(e);
            }
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");
        }
    }
}
