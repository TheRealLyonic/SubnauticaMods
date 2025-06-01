using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }
        
        public static AssetBundle AssetBundle { get; private set; }
        
        public static Config CONFIG { get; private set; }
        
        public static string PLUGIN_PATH { get; private set; }
        public static string MOD_FOLDER_PATH { get; private set; }

        private const string PLUGIN_GUID = "com.lyonicdevelopment.ultimatemateriallibrary";
        private const string PLUGIN_NAME = "Ultimate Material Library";
        private const string PLUGIN_VERSION = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(PLUGIN_GUID);

        private void Awake()
        {
            Logger = base.Logger;

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            PLUGIN_PATH = assemblyLocation.Substring(0, assemblyLocation.IndexOf("UltimateMaterialLibrary", StringComparison.Ordinal) - 1);
            MOD_FOLDER_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly.GetExecutingAssembly(), "uml_assets");

            LanguageHandler.RegisterLocalizationFolder();
            
            CONFIG = OptionsPanelHandler.RegisterModOptions<Config>();
            
            Harmony.PatchAll();
            
            Utility.MaterialDatabase.Initialize();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded.");
        }
    }
}