using System.Collections;
using System.Linq;
using LyonicDevelopment.IslandSpawn.MaterialUtil;
using LyonicDevelopment.IslandSpawn.Mono;
using LyonicDevelopment.IslandSpawn.Mono.Prefabs;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn.Core
{
    public static class PrefabRegister
    {
        public static CustomPrefab rustedSolarPanel { get; private set; }

        public static readonly SpawnLocation[] solarPanelSpawnLocations =
        {
            new SpawnLocation(new Vector3(-804.3f, 79.44f, -1053.95f), new Vector3(0f, 240f, 0f)),
            new SpawnLocation(new Vector3(-802.95f, 79.44f, -1049.05f), new Vector3(0f, 325.714f, 0f))
        };
        
        public static CustomPrefab customFabricator { get; private set; }
        public static readonly SpawnLocation fabricatorSpawnLocation = new SpawnLocation(new Vector3(-804.82f, 78.1f, -1051.96f), new Vector3(0f, 106f, 0f));

        public static CustomPrefab customRadio { get; private set; }
        public static readonly SpawnLocation radioSpawnLocation = new SpawnLocation(new Vector3(-804.6f, 77.9f, -1050.7f), new Vector3(0f, 106.26f, 0f));

        public static CustomPrefab rustedMedCabinet { get; private set; }
        public static readonly SpawnLocation medCabinetSpawnLocation = new SpawnLocation(new Vector3(-802.445f, 78.1f, -1051.06f), new Vector3(0f, 285f, 0f));
        public static CustomPrefab powerCollider { get; private set; }
        public static readonly SpawnLocation colliderSpawnLocation = new SpawnLocation(new Vector3(-804f, 76.87f, -1050.71f), new Vector3(0f, 17.5f, 0f));
        
        public static CustomPrefab seedSack { get; private set; }
        public static readonly SpawnLocation seedSackSpawnLocation = new SpawnLocation(new Vector3(-795.4f, -2f, -1007.3f), new Vector3(0f, 343.19f, 0f));
        
        public static CustomPrefab mediumLocker { get; private set; }
        public static readonly SpawnLocation lockerSpawnLocation = new SpawnLocation(new Vector3(-801.7f, 76.39f, -1044.35f), new Vector3(0f, 194f, 0f));
        
        public static void RegisterPrefabs()
        {
            CoroutineHost.StartCoroutine(RegisterRustedSolarPanel());
            RegisterCustomFabricator();
            RegisterCustomRadio();
            RegisterRustedMedCabinet();
            RegisterPowerCollider();
            RegisterSeedSack();
            RegisterMediumStorageLocker();
        }

        private static IEnumerator RegisterRustedSolarPanel()
        {
            rustedSolarPanel = new CustomPrefab(PrefabInfo.WithTechType("RustedSolarPanel", "Solar Panel", ""));
            var rustedObject = Plugin.AssetBundle.LoadAsset<GameObject>("RustedSolarPanel.prefab");

            rustedObject.GetComponent<TechTag>().type = rustedSolarPanel.Info.TechType;

            var solarPanelTask = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);
            yield return solarPanelTask;
            var vanillaSolarPanel = solarPanelTask.GetResult();

            rustedObject.GetComponent<PowerRelay>().powerSystemPreviewPrefab =
                vanillaSolarPanel.GetComponent<PowerRelay>().powerSystemPreviewPrefab;
            rustedObject.GetComponent<PowerFX>().vfxPrefab = vanillaSolarPanel.GetComponent<PowerFX>().vfxPrefab;
            
            MaterialUtils.ApplySNShaders(rustedObject);
            
            rustedSolarPanel.SetGameObject(rustedObject);
            rustedSolarPanel.SetSpawns(solarPanelSpawnLocations);
            rustedSolarPanel.Register();
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
                
                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
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
                
                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
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

        private static void RegisterRustedMedCabinet()
        {
            rustedMedCabinet = new CustomPrefab(PrefabInfo.WithTechType("RustedMedCabinet", "Medical Kit Fabricator", ""));
            var rustedObject = Plugin.AssetBundle.LoadAsset<GameObject>("RustedMedCabinet.prefab");
            
            rustedObject.GetComponent<TechTag>().type = rustedMedCabinet.Info.TechType;
            
            var rustedCabinetComponent = rustedObject.GetComponent<RustedMedicalCabinet>();

            rustedCabinetComponent.openSFX.asset = AudioUtils.GetFmodAsset("event:/sub/base/medkit_locker/open", "{312b0baa-3779-4889-bb3f-6739199c9237}");
            rustedCabinetComponent.closeSFX.asset = AudioUtils.GetFmodAsset("event:/sub/base/medkit_locker/close", "{dc9ccdcb-273a-4d37-89e0-95bd963927d1}");
            rustedCabinetComponent.playSound.asset = AudioUtils.GetFmodAsset("event:/sub_module/first_aid/spawn", "{b6bd881c-725c-4cd3-9a79-9f7256368b58}");

            MaterialUtils.ApplySNShaders(rustedObject);
            ApplyMaterialModification.ApplyAllModifications(rustedObject);
            
            rustedMedCabinet.SetGameObject(rustedObject);
            rustedMedCabinet.SetSpawns(medCabinetSpawnLocation);
            rustedMedCabinet.Register();
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
            var seedSackObject = Plugin.AssetBundle.LoadAsset<GameObject>("Seed_Sack.prefab");
            
            MaterialUtils.ApplySNShaders(seedSackObject);
            ApplyMaterialModification.ApplyAllModifications(seedSackObject);
            
            seedSack.SetGameObject(seedSackObject);
            seedSack.SetSpawns(seedSackSpawnLocation);
            seedSack.Register();
        }

        private static void RegisterMediumStorageLocker()
        {
            mediumLocker = new CustomPrefab(PrefabInfo.WithTechType("MediumStorageLocker", "Medium Locker", ""));
            var lockerObject = Plugin.AssetBundle.LoadAsset<GameObject>("Medium_Storage_Locker");

            lockerObject.GetComponentInChildren<ChildObjectIdentifier>().classId = mediumLocker.Info.ClassID;

            MaterialUtils.ApplySNShaders(lockerObject);
            ApplyMaterialModification.ApplyAllModifications(lockerObject);
            
            mediumLocker.SetGameObject(lockerObject);
            mediumLocker.SetSpawns(lockerSpawnLocation);
            mediumLocker.Register();
        }

    }
}