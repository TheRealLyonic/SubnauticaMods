using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }

        public static CustomPrefab customFabricator;
        public static SpawnLocation fabSpawnLocation = new SpawnLocation(new Vector3(-804.9f, 78.1f, -1051.96f), new Vector3(0f, 100f, 0f));

        public static CustomPrefab customSolarPanel;
        public static SpawnLocation panelSpawnLocation = new SpawnLocation(new Vector3(-804.1f, 79.45f, -1053.95f), new Vector3(0f, 240f, 0f));

        public static CustomPrefab powerCollider;
        public static SpawnLocation colliderSpawnLocation = new SpawnLocation(new Vector3(-804f, 76.87f, -1050.71f), new Vector3(0f, 17.5f, 0f));
        
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
            
            RegisterCustomFabricator();
            RegisterCustomSolarPanel();
            RegisterPowerCollider();
            
            Harmony.PatchAll();
            
            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded.");
        }

        private static void RegisterCustomFabricator()
        {
            customFabricator = new CustomPrefab(PrefabInfo.WithTechType("CustomFabricator"));
            
            var gameObjectTemplate = new CloneTemplate(customFabricator.Info, TechType.Fabricator);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                Destroy(prefab.GetComponent<PowerRelay>());
                Destroy(prefab.GetComponent<Constructable>());
                Destroy(prefab.GetComponent<PreventDeconstruction>());
                
                prefab.AddComponent<CustomPowerRelay>();
            };
            
            customFabricator.SetGameObject(gameObjectTemplate);
            customFabricator.SetSpawns(fabSpawnLocation);
            
            customFabricator.Register();
        }

        private static void RegisterCustomSolarPanel()
        {
            customSolarPanel = new CustomPrefab(PrefabInfo.WithTechType("CustomSolarPanel"));

            var gameObjectTemplate = new CloneTemplate(customSolarPanel.Info, TechType.SolarPanel);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                Destroy(prefab.GetComponent<PowerSource>());
                Destroy(prefab.GetComponent<Constructable>());
                
                prefab.AddComponent<CustomPowerSource>();
            };
            
            customSolarPanel.SetGameObject(gameObjectTemplate);
            customSolarPanel.SetSpawns(panelSpawnLocation);
            
            customSolarPanel.Register();
        }

        private static void RegisterPowerCollider()
        {
            powerCollider = new CustomPrefab(PrefabInfo.WithTechType("PowerCollider"));

            GameObject powerColliderObject = AssetBundle.LoadAsset<GameObject>("PowerCollider");
            
            PrefabUtils.AddBasicComponents(powerColliderObject, powerCollider.Info.ClassID, 
                powerCollider.Info.TechType, LargeWorldEntity.CellLevel.Near);
            
            powerColliderObject.transform.GetChild(0).gameObject.AddComponent<PowerCollider>();
            
            powerCollider.SetGameObject(powerColliderObject);
            powerCollider.SetSpawns(colliderSpawnLocation);

            powerCollider.Register();
        }
        
    }
}