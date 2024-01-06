using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }

        public static CustomPrefab customFabricator;
        public static SpawnLocation fabSpawnLocation = new SpawnLocation(new Vector3(0f, 0f, 0f));
        
        private const string PLUGIN_GUID = "com.lyonicdevelopment.islandspawn";
        private const string PLUGIN_NAME = "Island Spawn";
        private const string PLUGIN_VERSION = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(PLUGIN_GUID);
        
        private void Awake()
        {
            Logger = base.Logger;
            
            RegisterCustomFabricator();
            
            Harmony.PatchAll();
            
            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded.");
        }

        private static void RegisterCustomFabricator()
        {
            customFabricator = new CustomPrefab(PrefabInfo.WithTechType("CustomFabricator"));
            
            customFabricator.SetGameObject(new CloneTemplate(customFabricator.Info, TechType.Fabricator));
            customFabricator.SetSpawns(fabSpawnLocation);
            
            customFabricator.Register();
        }
        
    }
}