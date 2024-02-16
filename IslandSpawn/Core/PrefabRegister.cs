using LyonicDevelopment.IslandSpawn.MaterialUtil;
using LyonicDevelopment.IslandSpawn.Mono;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Core
{
    public static class PrefabRegister
    {
        public static CustomPrefab customSolarPanel { get; private set; }
        public static readonly SpawnLocation solarPanelSpawnLocation = new SpawnLocation(new Vector3(-804.1f, 79.45f, -1053.95f), new Vector3(0f, 240f, 0f));
        
        public static CustomPrefab customFabricator { get; private set; }
        public static readonly SpawnLocation fabricatorSpawnLocation = new SpawnLocation(new Vector3(-804.82f, 78.1f, -1051.96f), new Vector3(0f, 106f, 0f));

        public static CustomPrefab customRadio { get; private set; }
        public static readonly SpawnLocation radioSpawnLocation = new SpawnLocation(new Vector3(-804.6f, 77.9f, -1050.7f), new Vector3(0f, 106.26f, 0f));

        public static CustomPrefab customMedCabinet { get; private set; }
        public static readonly SpawnLocation medCabinetSpawnLocation = new SpawnLocation(new Vector3(-802.36f, 78.1f, -1051.06f), new Vector3(0f, 285f, 0f));

        public static CustomPrefab customLocker { get; private set; }
        public static readonly SpawnLocation lockerSpawnLocation = new SpawnLocation(new Vector3(-801.7f, 76.6f, -1044.2f), new Vector3(0f, 195f, 0f));
        
        public static CustomPrefab powerCollider { get; private set; }
        public static readonly SpawnLocation colliderSpawnLocation = new SpawnLocation(new Vector3(-804f, 76.87f, -1050.71f), new Vector3(0f, 17.5f, 0f));
        
        public static CustomPrefab seedSack { get; private set; }
        public static readonly SpawnLocation seedSackSpawnLocation = new SpawnLocation(new Vector3(-795.4f, -2f, -1007.3f), new Vector3(0f, 343.19f, 0f));
        
        public static void RegisterPrefabs()
        {
            RegisterCustomSolarPanel();
            RegisterCustomFabricator();
            RegisterCustomRadio();
            RegisterCustomMedCabinet();
            RegisterCustomLocker();
            RegisterPowerCollider();
            RegisterSeedSack();
        }

        private static void RegisterCustomSolarPanel()
        {
            customSolarPanel = new CustomPrefab(PrefabInfo.WithTechType("CustomSolarPanel", "Solar Panel", ""));
            
            var gameObjectTemplate = new CloneTemplate(customSolarPanel.Info, TechType.SolarPanel);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                GameObject.Destroy(prefab.GetComponent<PowerSource>());
                GameObject.Destroy(prefab.GetComponent<PreventDeconstruction>());

                prefab.GetComponent<Constructable>().deconstructionAllowed = false;
                prefab.GetComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
                
                prefab.AddComponent<CustomPowerSource>();
                
                foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
                {
                    foreach (var material in renderer.materials)
                    {
                        material.SetTexture(ShaderPropertyID._MainTex, Plugin.AssetBundle.LoadAsset<Texture2D>("Solar_Panel_Color"));
                        material.SetTexture(ShaderPropertyID._SpecTex, Plugin.AssetBundle.LoadAsset<Texture2D>("Solar_Panel_Color"));
                    }
                }
            };
            
            customSolarPanel.SetGameObject(gameObjectTemplate);
            customSolarPanel.SetSpawns(solarPanelSpawnLocation);
            
            customSolarPanel.Register();
        }
        
        private static void RegisterCustomFabricator()
        {
            customFabricator = new CustomPrefab(PrefabInfo.WithTechType("CustomFabricator", "Fabricator", ""));
            
            var gameObjectTemplate = new CloneTemplate(customFabricator.Info, TechType.Fabricator);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                GameObject.Destroy(prefab.GetComponent<PowerRelay>());
                GameObject.Destroy(prefab.GetComponent<PreventDeconstruction>());

                prefab.GetComponent<Constructable>().deconstructionAllowed = false;
                prefab.GetComponent<Constructable>().techType = TechType.Fabricator;
                
                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
                prefab.AddComponent<CustomPowerRelay>();
                
                foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
                {
                    foreach (var material in renderer.materials)
                    {
                        material.SetTexture(ShaderPropertyID._MainTex, Plugin.AssetBundle.LoadAsset<Texture2D>("Fabricator_Color"));
                        material.SetTexture(ShaderPropertyID._SpecTex, Plugin.AssetBundle.LoadAsset<Texture2D>("Fabricator_Spec"));
                        material.SetTexture(ShaderPropertyID._Illum, Plugin.AssetBundle.LoadAsset<Texture2D>("Fabricator_Illum"));
                    }
                }
            };
            
            customFabricator.SetGameObject(gameObjectTemplate);
            customFabricator.SetSpawns(fabricatorSpawnLocation);
            
            customFabricator.Register();
        }

        private static void RegisterCustomRadio()
        {
            customRadio = new CustomPrefab(PrefabInfo.WithTechType("CustomRadio", "Radio", ""));
            
            var gameObjectTemplate = new CloneTemplate(customRadio.Info, TechType.Radio);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                GameObject.Destroy(prefab.GetComponent<PreventDeconstruction>());

                prefab.GetComponent<LiveMixin>().health = 20;
                prefab.GetComponent<Constructable>().deconstructionAllowed = false;
                
                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
                prefab.AddComponent<CustomPowerRelay>();
                
                foreach (var material in prefab.GetComponentsInChildren<Renderer>(true)[0].materials)
                {
                    material.SetTexture(ShaderPropertyID._MainTex, Plugin.AssetBundle.LoadAsset<Texture2D>("Radio_Color"));
                    material.SetTexture(ShaderPropertyID._SpecTex, Plugin.AssetBundle.LoadAsset<Texture2D>("Radio_Color"));
                    material.SetTexture(ShaderPropertyID._Illum, Plugin.AssetBundle.LoadAsset<Texture2D>("Radio_Illum"));
                }
            };
            
            customRadio.SetGameObject(gameObjectTemplate);
            customRadio.SetSpawns(radioSpawnLocation);

            customRadio.Register();
        }

        private static void RegisterCustomMedCabinet()
        {
            customMedCabinet = new CustomPrefab(PrefabInfo.WithTechType("CustomMedCabinet", "Medical Kit Fabricator", ""));
            
            var gameObjectTemplate = new CloneTemplate(customMedCabinet.Info, TechType.MedicalCabinet);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                GameObject.Destroy(prefab.GetComponent<PreventDeconstruction>());

                prefab.GetComponent<Constructable>().deconstructionAllowed = false;
                
                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
                prefab.AddComponent<CustomPowerRelay>();
                
                for (int i = 0; i < 2; i++)
                {
                    prefab.GetComponentsInChildren<Renderer>()[i].materials[0].SetTexture(ShaderPropertyID._MainTex, Plugin.AssetBundle.LoadAsset<Texture2D>("Medical_Cabinet_Color"));
                    prefab.GetComponentsInChildren<Renderer>()[i].materials[0].SetTexture(ShaderPropertyID._SpecTex, Plugin.AssetBundle.LoadAsset<Texture2D>("Medical_Cabinet_Spec"));
                    prefab.GetComponentsInChildren<Renderer>()[i].materials[0].SetTexture(ShaderPropertyID._Illum, Plugin.AssetBundle.LoadAsset<Texture2D>("Medical_Cabinet_Illum"));
                }
            };
            
            customMedCabinet.SetGameObject(gameObjectTemplate);
            customMedCabinet.SetSpawns(medCabinetSpawnLocation);
            
            customMedCabinet.Register();
        }

        private static void RegisterCustomLocker()
        {
            customLocker = new CustomPrefab(PrefabInfo.WithTechType("CustomLocker", "Locker", ""));

            var gameObjectTemplate = new CloneTemplate(customLocker.Info, TechType.Locker);

            gameObjectTemplate.ModifyPrefab += (prefab) =>
            {
                prefab.GetComponent<Constructable>().deconstructionAllowed = false;

                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
            };
            
            customLocker.SetGameObject(gameObjectTemplate);
            customLocker.SetSpawns(lockerSpawnLocation);
            
            customLocker.Register();
        }

        private static void RegisterPowerCollider()
        {
            powerCollider = new CustomPrefab(PrefabInfo.WithTechType("PowerCollider"));
            
            powerCollider.SetGameObject(Plugin.AssetBundle.LoadAsset<GameObject>("PowerCollider"));
            powerCollider.SetSpawns(colliderSpawnLocation);

            powerCollider.Register();
        }

        private static void RegisterSeedSack()
        {
            seedSack = new CustomPrefab(PrefabInfo.WithTechType("SeedSack", "Seed sack", ""));
            
            var seedSackObject = Plugin.AssetBundle.LoadAsset<GameObject>("Seed_Sack");
            
            MaterialUtils.ApplySNShaders(seedSackObject, modifiers: new SeedSackMatSettings());
            
            seedSack.SetGameObject(seedSackObject);
            seedSack.SetSpawns(seedSackSpawnLocation);
            
            seedSack.Register();
        }

    }
}