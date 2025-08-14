using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;
using LyonicDevelopment.IslandSpawn.Core;
using Nautilus.Handlers;

namespace LyonicDevelopment.IslandSpawn
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    [BepInDependency("Esper89.TerrainPatcher")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }
        
        public static AssetBundle AssetBundle { get; private set; }
        
        private const string PLUGIN_GUID = "com.lyonicdevelopment.islandspawn";
        private const string PLUGIN_NAME = "Island Spawn";
        private const string PLUGIN_VERSION = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(PLUGIN_GUID);
        
        private void Awake()
        {
            Logger = base.Logger;

            AssetBundle =
                AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly.GetExecutingAssembly(), "islandspawnassets");
            
            WaitScreenHandler.RegisterLateLoadTask(PLUGIN_NAME, task =>
            {
                PrefabRegister.RegisterPrefabs();
                DistributionHandler.RegisterDistribution();
                StoryManager.RegisterStoryEvents();
            }, "Registering Mod Content");
            
            Harmony.PatchAll();
            
            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded.");
        }
        
    }
}