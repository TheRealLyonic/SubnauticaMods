using System;
using LyonicDevelopment.IslandSpawn.Mono;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Extensions;
using Nautilus.Handlers;
using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
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

        private static void RegisterPowerCollider()
        {
            powerCollider = new CustomPrefab(PrefabInfo.WithTechType("PowerCollider"));
            
            GameObject powerColliderObject = Plugin.AssetBundle.LoadAsset<GameObject>("PowerCollider");
            
            PrefabUtils.AddBasicComponents(powerColliderObject, powerCollider.Info.ClassID, 
                powerCollider.Info.TechType, LargeWorldEntity.CellLevel.Medium);
            
            powerColliderObject.transform.GetChild(0).gameObject.AddComponent<PowerCollider>();
            powerColliderObject.transform.GetChild(0).gameObject.AddComponent<PrefabDestroyer>();
            
            powerCollider.SetGameObject(powerColliderObject);
            powerCollider.SetSpawns(colliderSpawnLocation);

            powerCollider.Register();
        }

        private static void RegisterSeedSack()
        {
            seedSack = new CustomPrefab(PrefabInfo.WithTechType("SeedSack", "Seed sack", ""));
            
            var seedSackObject = Plugin.AssetBundle.LoadAsset<GameObject>("Seed_Sack");
            
            PrefabUtils.AddBasicComponents(seedSackObject, seedSack.Info.ClassID, TechType.None, LargeWorldEntity.CellLevel.Medium);
            
            seedSackObject.EnsureComponent<SkyApplier>().renderers =
                seedSackObject.GetAllComponentsInChildren<Renderer>();
            
            seedSackObject.AddComponent<SeedSackController>();

            var fruitObject = seedSackObject.transform.GetChild(1);

            PickPrefab[] fruit = new PickPrefab[fruitObject.childCount];
            
            //Add the pickprefab component to all the fruit on the cluster.
            for (int i = 0; i < fruitObject.childCount; i++)
            {
                var component = fruitObject.GetChild(i).gameObject.AddComponent<PickPrefab>();

                component.pickTech = TechType.CreepvineSeedCluster;

                fruit[i] = component;
            }

            seedSackObject.AddComponent<FruitPlant>().fruits = fruit;
            
            //Fix materials
            MaterialUtils.ApplySNShaders(seedSackObject);

            var kelpMaterial = seedSackObject.transform.GetChild(0).GetComponent<MeshRenderer>().materials[0];
            
            kelpMaterial.SetColor("_GlowColor", new Color(0.666f, 0.952f, 0f));
            kelpMaterial.SetColor("_SpecColor", new Color(0.558f, 1f, 0.689f));
            
            kelpMaterial.SetFloat("_MarmoSpecEnum", 3f);
            kelpMaterial.SetFloat("_Shininess", 3.73f);
            kelpMaterial.SetFloat("_Fresnel", 0f);
            kelpMaterial.SetFloat("_GlowStrength", 1f);
            kelpMaterial.SetFloat("_GlowStrengthNight", 0.2f);
            
            foreach (var renderer in fruitObject.GetComponentsInChildren<Renderer>())
            {
                foreach (var material in renderer.materials)
                {
                    material.SetColor("_SpecColor", new Color(0.556f, 1f, 0.689f));
                    
                    material.SetFloat("_Shininess", 7.1718f);
                    material.SetFloat("_Fresnel", 0.3571f);
                    material.SetFloat("_GlowStrength", 1f);
                    material.SetFloat("_GlowStrengthNight", 1f);
                    material.SetFloat("_LightmapStrength", 2.65f);
                }
            }
            
            seedSack.SetGameObject(seedSackObject);
            seedSack.SetSpawns(seedSackSpawnLocation);
            
            seedSack.Register();

            string entryDesc = "A strange adaptation of the regular creepvine, evolved to grow on the walls of underwater caves.\n\nAssessment: Vital alien resource - Construction Applications";
            
            PDAHandler.AddEncyclopediaEntry("SeedSackEncy", "Lifeforms/Flora/Exploitable", "Seed Sack", entryDesc, Plugin.AssetBundle.LoadAsset<Texture2D>("seed_sack_databank"));
            PDAHandler.AddCustomScannerEntry(seedSack.Info.TechType, 3f, false, "SeedSackEncy");
        }

    }
}