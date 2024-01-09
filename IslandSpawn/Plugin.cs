using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    [BepInDependency("Esper89.TerrainPatcher")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }

        public static CustomPrefab customFabricator;
        public static readonly SpawnLocation fabricatorSpawnLocation = new SpawnLocation(new Vector3(-804.9f, 78.1f, -1051.96f), new Vector3(0f, 100f, 0f));

        public static CustomPrefab customSolarPanel;
        public static readonly SpawnLocation solarPanelSpawnLocation = new SpawnLocation(new Vector3(-804.1f, 79.45f, -1053.95f), new Vector3(0f, 240f, 0f));

        public static CustomPrefab customRadio;
        public static readonly SpawnLocation radioSpawnLocation = new SpawnLocation(new Vector3(-804.6f, 77.9f, -1050.7f), new Vector3(0f, 106.26f, 0f));

        public static CustomPrefab customMedCabinet;
        public static readonly SpawnLocation medCabinetSpawnLocation = new SpawnLocation(new Vector3(-802.36f, 78.1f, -1051.06f), new Vector3(0f, 285f, 0f));

        public static CustomPrefab powerCollider;
        public static readonly SpawnLocation colliderSpawnLocation = new SpawnLocation(new Vector3(-804f, 76.87f, -1050.71f), new Vector3(0f, 17.5f, 0f));
        
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
            RegisterCustomRadio();
            RegisterCustomMedicalCabinet();
            RegisterPowerCollider();
            RegisterCustomLoot();
            
            Harmony.PatchAll();
            
            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded.");
        }

        public static void RegisterCustomFabricator()
        { 
            customFabricator = new CustomPrefab(PrefabInfo.WithTechType("CustomFabricator"));
            
            var gameObjectTemplate = new CloneTemplate(customFabricator.Info, TechType.Fabricator);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                Destroy(prefab.GetComponent<PowerRelay>());
                Destroy(prefab.GetComponent<PreventDeconstruction>());

                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
                prefab.AddComponent<PreventDeconstructionAlways>().always = true;
                prefab.AddComponent<CustomPowerRelay>();
            };
            
            customFabricator.SetGameObject(gameObjectTemplate);
            customFabricator.SetSpawns(fabricatorSpawnLocation);
            
            customFabricator.Register();
        }

        public static void RegisterCustomSolarPanel()
        {
            customSolarPanel = new CustomPrefab(PrefabInfo.WithTechType("CustomSolarPanel"));
            
            var gameObjectTemplate = new CloneTemplate(customSolarPanel.Info, TechType.SolarPanel);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                Destroy(prefab.GetComponent<PowerSource>());
                Destroy(prefab.GetComponent<PreventDeconstruction>());

                prefab.AddComponent<PreventDeconstructionAlways>().always = true;
                prefab.AddComponent<CustomPowerSource>();
            };
            
            customSolarPanel.SetGameObject(gameObjectTemplate);
            customSolarPanel.SetSpawns(solarPanelSpawnLocation);
            
            customSolarPanel.Register();
        }

        public static void RegisterCustomRadio()
        {
            customRadio = new CustomPrefab(PrefabInfo.WithTechType("CustomRadio"));
            
            var gameObjectTemplate = new CloneTemplate(customRadio.Info, TechType.Radio);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                Destroy(prefab.GetComponent<PreventDeconstruction>());

                prefab.GetComponent<LiveMixin>().health = 25;

                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
                prefab.AddComponent<PreventDeconstructionAlways>().always = true;
                prefab.AddComponent<CustomPowerRelay>();
            };
            
            customRadio.SetGameObject(gameObjectTemplate);
            customRadio.SetSpawns(radioSpawnLocation);

            customRadio.Register();
        }

        public static void RegisterCustomMedicalCabinet()
        {
            customMedCabinet = new CustomPrefab(PrefabInfo.WithTechType("CustomMedicalCabinet"));
            
            var gameObjectTemplate = new CloneTemplate(customMedCabinet.Info, TechType.MedicalCabinet);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                Destroy(prefab.GetComponent<PreventDeconstruction>());

                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
                prefab.AddComponent<PreventDeconstructionAlways>().always = true;
                prefab.AddComponent<CustomPowerRelay>();
            };
            
            customMedCabinet.SetGameObject(gameObjectTemplate);
            customMedCabinet.SetSpawns(medCabinetSpawnLocation);
            
            customMedCabinet.Register();
        }

        public static void RegisterPowerCollider()
        {
            powerCollider = new CustomPrefab(PrefabInfo.WithTechType("PowerCollider"));
            
            GameObject powerColliderObject = AssetBundle.LoadAsset<GameObject>("PowerCollider");
            
            PrefabUtils.AddBasicComponents(powerColliderObject, powerCollider.Info.ClassID, 
                powerCollider.Info.TechType, LargeWorldEntity.CellLevel.Near);
            
            powerColliderObject.transform.GetChild(0).gameObject.AddComponent<PowerCollider>();
            powerColliderObject.transform.GetChild(0).gameObject.AddComponent<VanillaPrefabSpawner>();
            
            powerCollider.SetGameObject(powerColliderObject);
            powerCollider.SetSpawns(colliderSpawnLocation);

            powerCollider.Register();
        }

        private static void RegisterCustomLoot()
        {
            //Note that none of the floating island biometypes have valid resource spawns except for the two that are inside and outside of the degasi habitats.
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.FloatingIslands_AbandonedBase_Outside, 0.2f, 1);
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.SandstoneChunk), BiomeType.FloatingIslands_AbandonedBase_Outside, 0.1f, 1);
        }
        
    }
}