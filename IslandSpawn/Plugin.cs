using System.Collections.Generic;
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
        public static readonly SpawnLocation fabricatorSpawnLocation = new SpawnLocation(new Vector3(-804.82f, 78.1f, -1051.96f), new Vector3(0f, 106f, 0f));

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
            RegisterVanillaSpawns();
            RegisterCustomLoot();
            RegisterCustomPDAEntries();
            
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
                
                foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
                {
                    foreach (var material in renderer.materials)
                    {
                        material.SetTexture("_MainTex", AssetBundle.LoadAsset<Texture2D>("Fabricator_Color"));
                        material.SetTexture("_SpecTex", AssetBundle.LoadAsset<Texture2D>("Fabricator_Spec"));
                        material.SetTexture("_Illum", AssetBundle.LoadAsset<Texture2D>("Fabricator_Illum"));
                    }
                }
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
                
                foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
                {
                    foreach (var material in renderer.materials)
                    {
                        material.SetTexture("_MainTex", AssetBundle.LoadAsset<Texture2D>("Solar_Panel_Color"));
                        material.SetTexture("_SpecTex", AssetBundle.LoadAsset<Texture2D>("Solar_Panel_Color"));
                    }
                }
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

                foreach (var material in prefab.GetComponentsInChildren<Renderer>(true)[0].materials)
                {
                    material.SetTexture("_MainTex", AssetBundle.LoadAsset<Texture2D>("Radio_Color"));
                    material.SetTexture("_SpecTex", AssetBundle.LoadAsset<Texture2D>("Radio_Color"));
                    material.SetTexture("_Illum", AssetBundle.LoadAsset<Texture2D>("Radio_Illum"));
                }
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
                
                for (int i = 0; i < 2; i++)
                {
                    prefab.GetComponentsInChildren<Renderer>()[i].materials[0].SetTexture("_MainTex", AssetBundle.LoadAsset<Texture2D>("Medical_Cabinet_Color"));
                    prefab.GetComponentsInChildren<Renderer>()[i].materials[0].SetTexture("_SpecTex", AssetBundle.LoadAsset<Texture2D>("Medical_Cabinet_Spec"));
                    prefab.GetComponentsInChildren<Renderer>()[i].materials[0].SetTexture("_Illum", AssetBundle.LoadAsset<Texture2D>("Medical_Cabinet_Illum"));
                }
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
            
            powerCollider.SetGameObject(powerColliderObject);
            powerCollider.SetSpawns(colliderSpawnLocation);

            powerCollider.Register();
        }

        private static void RegisterVanillaSpawns()
        {
            string tableCoralID = "70eb6270-bf5e-4d6a-8182-484ffcfd8de6";
            
            var vanillaSpawnInfo = new List<SpawnInfo>
            {
                //Crash homes
                new SpawnInfo(TechType.CrashHome, new Vector3(-660.66f, -11.3f, -1026.17f), new Quaternion(0.43f, 0f, 0f, 0.90f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-688.64f, -10f, -993.64f), new Quaternion(0.56f, 0f, 0f, 0.83f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-689.87f, -13.12f, -984.8f), new Quaternion(0f, 0f, -0.23f, 0.97f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-662.3f, -14.8f, -1012.82f), new Quaternion(0f, 0f, -0.23f, 0.97f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-731.8f, -12.49f, -948.19f), new Quaternion(0f, 0f, -0.56f, 0.83f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-778.65f, -7.07f, -977.25f), new Quaternion(0.47f, 0f, 0f, 0.88f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-830.28f, -14.3f, -1007.15f), new Quaternion(0.56f, 0f, 0f, 0.83f)),
                
                //Table coral - Batch 1
                new SpawnInfo(tableCoralID, new Vector3(-735.7f, -29.3f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
                new SpawnInfo(tableCoralID, new Vector3(-735.7f, -29.6f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
                new SpawnInfo(tableCoralID, new Vector3(-735.7f, -30.0f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
                //Table coral - Batch 2
                new SpawnInfo(tableCoralID, new Vector3(-756f, -27.9f, -951f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                new SpawnInfo(tableCoralID, new Vector3(-756f, -28.2f, -951f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                new SpawnInfo(tableCoralID, new Vector3(-755.9f, -28.5f, -951f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                //Table coral - Batch 3
                new SpawnInfo(tableCoralID, new Vector3(-755.5f, -27.9f, -949.5f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                new SpawnInfo(tableCoralID, new Vector3(-755.5f, -28.2f, -949.5f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                new SpawnInfo(tableCoralID, new Vector3(-755.4f, -28.5f, -949.5f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                //Table coral - Batch 4 (Up)
                new SpawnInfo(tableCoralID, new Vector3(-859.9f, -39.1f, -1038.6f), new Quaternion(0f, 0.14f, 0f, 0.99f)),
                //Table coral - Batch 4 (Main)
                new SpawnInfo(tableCoralID, new Vector3(-860.3f, -40.6f, -1038.6f), new Quaternion(0f, 0.14f, 0f, 0.99f)),
                new SpawnInfo(tableCoralID, new Vector3(-860.3f, -40.9f, -1038.6f), new Quaternion(0f, 0.14f, 0f, 0.99f)),
                new SpawnInfo(tableCoralID, new Vector3(-860.3f, -41.2f, -1038.65f), new Quaternion(0f, 0.14f, 0f, 0.99f)),
                //Table coral - Batch 5
                new SpawnInfo(tableCoralID, new Vector3(-762f, -44.0f, -1036.75f), new Quaternion(0f, 0f, 0f, 1f)),
                new SpawnInfo(tableCoralID, new Vector3(-762f, -44.3f, -1036.75f), new Quaternion(0f, 0f, 0f, 1f)),
                new SpawnInfo(tableCoralID, new Vector3(-762f, -44.6f, -1036.75f), new Quaternion(0f, 0f, 0f, 1f)),
                new SpawnInfo(tableCoralID, new Vector3(-762f, -44.9f, -1036.8f), new Quaternion(0f, 0f, 0f, 1f))
            };
            
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawns(vanillaSpawnInfo);
        }

        private static void RegisterCustomLoot()
        {
            //The GetClassIdForTechType method won't work with these objects, despite them having their own techtypes.
            string beaconFragmentClassID = "a50c91eb-f7cf-4fbf-8157-0aa8d444820c";
            string gravtrapFragmentClassID = "6e4f85c2-ad1d-4d0a-b20c-1158204ee424";
            
            //Note that none of the floating island biometypes have valid resource spawns except for the two that are inside and outside of the degasi habitats.
            LootDistributionHandler.EditLootDistributionData(beaconFragmentClassID, BiomeType.FloatingIslands_AbandonedBase_Outside, 0.08f, 1);
            LootDistributionHandler.EditLootDistributionData(gravtrapFragmentClassID, BiomeType.FloatingIslands_AbandonedBase_Outside, 0.07f, 1);
            //The sparse reef is just below the floating island; Good place for fragments + Additional loot spawns?
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.SparseReef_Spike, 0.2f, 1);
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.SandstoneChunk), BiomeType.SparseReef_Spike, 0.1f, 1);
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.SparseReef_Wall, 0.2f, 1);
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.SeaglideFragment), BiomeType.SparseReef_Sand, 0.4f, 1);
        }

        private static void RegisterCustomPDAEntries()
        {
            string islandScanDesc = "Short range scans reveal multiple tunnel-systems running through the interior of this island. There appear to be small,"
                + " underwater access points located below the island's beaches. Whether these tunnels were made naturally, or artificially, is unclear.";
            
            PDAHandler.AddEncyclopediaEntry("islandScan", "PlanetaryGeology", "Island Scan Data", islandScanDesc, AssetBundle.LoadAsset<Texture2D>("island_databank_hint"));
            
            StoryGoalHandler.RegisterBiomeGoal("islandScan", Story.GoalType.Encyclopedia, "FloatingIsland", 10f);
        }
        
    }
}