using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.PlayerAnimationLibrary
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }
        
        public static AssetBundle AssetBundle { get; private set; }

        private const string PLUGIN_GUID = "com.lyonicdevelopment.playeranimationlibrary";
        private const string PLUGIN_NAME = "PlayerAnimationLibrary";
        private const string PLUGIN_VERSION = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(PLUGIN_GUID);

        private void Awake()
        {
            Logger = base.Logger;

            AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly.GetExecutingAssembly(), "pal_assets");

            Harmony.PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded.");
        }
    }
}