using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using Logger = QModManager.Utility.Logger;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Handlers;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

namespace ImprovedReaper
{
    [QModCore]
    public class MainPatcher : MonoBehaviour
    {

        public static GameObject asset;
        internal static Config config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"JDev_{assembly.GetName().Name}");

            //ENHANCED CRAGFIELD START

            //Reaper spawns
            //A post was made complaining of 'Massive FPS drops after killing some of the reapers', what could be causing this? Something with the fact that the spawns are manual and hardcoded as opposed to genereted in-game?
            //Workshop this later.
            Vector3[] reaperSpawnLocations =
            {
                new Vector3(-130f, -91f, -885f),
                new Vector3(-77f, -151f, -1068f),
                new Vector3(211f, -177f, -1244f),
                new Vector3(512f, -214f, -1267f),
                new Vector3(387f, -260f, -1558f),
                new Vector3(93f, -264f, -1483f),
                new Vector3(-170f, -167f, -1332f)
            };

            foreach(Vector3 spawnLocation in reaperSpawnLocations)
			{
                CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.ReaperLeviathan, spawnLocation));
			}

            //Loot distribution
            IDictionary<string, Tuple<float, BiomeType>> addedLoot = new Dictionary<string, Tuple<float, BiomeType>>
            {
                //Group 1 - Spawns on the rock
                { CraftData.GetClassIdForTechType(TechType.LimestoneChunk), Tuple.Create(1.5f, BiomeType.CragField_Rock) }, 
                { CraftData.GetClassIdForTechType(TechType.Diamond), Tuple.Create(0.5f, BiomeType.CragField_Rock) },
                { CraftData.GetClassIdForTechType(TechType.AluminumOxide), Tuple.Create(0.5f, BiomeType.CragField_Rock) },
                //Group 2 - Spawns on the ground
                { CraftData.GetClassIdForTechType(TechType.SandstoneChunk), Tuple.Create(3.0f, BiomeType.CragField_Ground) },
                { CraftData.GetClassIdForTechType(TechType.Lithium), Tuple.Create(0.5f, BiomeType.CragField_Ground) },
                { CraftData.GetClassIdForTechType(TechType.Nickel), Tuple.Create(0.3f, BiomeType.CragField_Ground) },
                { CraftData.GetClassIdForTechType(TechType.MoonpoolFragment), Tuple.Create(0.8f, BiomeType.CragField_Ground) },
                { CraftData.GetClassIdForTechType(TechType.ExosuitDrillArmFragment), Tuple.Create(0.8f, BiomeType.CragField_Ground) },
                { CraftData.GetClassIdForTechType(TechType.ExosuitGrapplingArmFragment), Tuple.Create(0.8f, BiomeType.CragField_Ground) },
                //Group 3 - Spawns in creature-only zone
                { CraftData.GetClassIdForTechType(TechType.Rockgrub), Tuple.Create(0.8f, BiomeType.CragField_OpenDeep_CreatureOnly) }
            };

            foreach(var loot in addedLoot)
			{
                LootDistributionHandler.EditLootDistributionData(loot.Key, loot.Value.Item2, loot.Value.Item1, 1);
            }

            //Adding Terrain Patch
            var cragFieldTerrainPatch = assembly.GetManifestResourceStream("ImprovedReaper.CragfieldTerrainPatch.optoctreepatch");

            TerrainPatcher.TerrainRegistry.PatchTerrain(cragFieldTerrainPatch);

            //ENHANCED CRAGFIELD END

            //TESTCUBE CODE START

            GameObject myAsset;

            string modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(modPath, "assets/testcubebundle"));

            if (myLoadedAssetBundle == null)
            {
                Logger.Log(Logger.Level.Error, "Failed to load AssetBundle!");
                Logger.Log(Logger.Level.Error, "**You're missing the testcubebundle file in ImprovedReaper/assets**");
                Logger.Log(Logger.Level.Error, Path.Combine(modPath, "assets/testcubebundle"));
                Application.Quit();
                return;
            }

            System.Object[] arr = myLoadedAssetBundle.LoadAllAssets();

            myAsset = null;

            foreach (System.Object obj in arr)
            {

                if (obj.ToString().Contains("TestCube"))
                {
                    myAsset = (GameObject)obj;
                }

            }

            myAsset.gameObject.SetActive(false);

            asset = myAsset;

            ConsoleCommandsHandler.Main.RegisterConsoleCommand("testcube", typeof(MainPatcher), nameof(TestCommand));

            //TESTCUBE CODE END

            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(assembly);
            Logger.Log(Logger.Level.Debug, "Successfully patched mod!");
		}

        public static void TestCommand()
        {
            GameObject currentCube = Instantiate(MainPatcher.asset);

            currentCube.SetActive(true);

            currentCube.transform.position = Player.main.transform.position;
        }

        [Menu("Reaper Damage")]
        public class Config : ConfigFile
		{
            //Slider for the multiple at which to increase the reaper's player damage output
            [Slider("Player Damage Multiplier", Format = "{0:F2}", Min = 1.0f, Max = 5.0f, DefaultValue = 2.0f, Step = 0.1f)]
            public float damageMultiplier = 2.0f;

            //Slider for the multiple at which to increase the reaper's cyclops damage output
            [Slider("Cyclops Damage Multiplier", Format = "{0:F2}", Min = 1.0f, Max = 5.0f, DefaultValue = 2.0f, Step = 0.1f)]
            public float cyclopsDamageMultiplier = 2.0f;

            //Slider for the multiple at which to increase the reaper's seamoth & exosuit damage output
            [Slider("Seamoth & PRAWN Damage Multiplier", Format = "{0:F2}", Min = 1.0f, Max = 5.0f, DefaultValue = 2.0f, Step = 0.1f)]
            public float seamothDamageMultiplier = 2.0f;
		}

    }
}