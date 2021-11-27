using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using Logger = QModManager.Utility.Logger;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Handlers;
using UnityEngine;
using System.IO;

namespace IncreaseReaperDamage
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
            Vector3 reaperSpawnLocation1 = new Vector3(92f, -262f, -1516f);
            Vector3 reaperSpawnLocation2 = new Vector3(138f, -355f, -1601f);
            Vector3 reaperSpawnLocation3 = new Vector3(431f, -234f, -1170f);
            Vector3 reaperSpawnLocation4 = new Vector3(574f, -269f, -1485f);

            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.ReaperLeviathan, reaperSpawnLocation1));
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.ReaperLeviathan, reaperSpawnLocation2));
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.ReaperLeviathan, reaperSpawnLocation3));
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.ReaperLeviathan, reaperSpawnLocation4));

            //Loot distribution
            var limestoneClassId = CraftData.GetClassIdForTechType(TechType.LimestoneChunk);
            var sandstoneClassId = CraftData.GetClassIdForTechType(TechType.SandstoneChunk);
            var diamondClassId = CraftData.GetClassIdForTechType(TechType.Diamond);
            var lithiumClassId = CraftData.GetClassIdForTechType(TechType.Lithium);
            var rubyClassId = CraftData.GetClassIdForTechType(TechType.AluminumOxide);
            var nickelClassId = CraftData.GetClassIdForTechType(TechType.Nickel);
            var moonpoolFragmentClassId = CraftData.GetClassIdForTechType(TechType.MoonpoolFragment);
            var drillArmFragmentClassId = CraftData.GetClassIdForTechType(TechType.ExosuitDrillArmFragment);
            var grappleArmFragmentClassId = CraftData.GetClassIdForTechType(TechType.ExosuitGrapplingArmFragment);
            var rockGrubClassId = CraftData.GetClassIdForTechType(TechType.Rockgrub);
            var oculusClassId = CraftData.GetClassIdForTechType(TechType.Oculus);

            LootDistributionHandler.EditLootDistributionData(limestoneClassId, BiomeType.CragField_Rock, 1.5f, 1);
            LootDistributionHandler.EditLootDistributionData(sandstoneClassId, BiomeType.CragField_Ground, 3.0f, 1);
            LootDistributionHandler.EditLootDistributionData(diamondClassId, BiomeType.CragField_Rock, 0.5f, 1);
            LootDistributionHandler.EditLootDistributionData(lithiumClassId, BiomeType.CragField_Ground, 0.2f, 1);
            LootDistributionHandler.EditLootDistributionData(rubyClassId, BiomeType.CragField_Rock, 0.2f, 1);
            LootDistributionHandler.EditLootDistributionData(nickelClassId, BiomeType.CragField_Ground, 0.3f, 1);
            LootDistributionHandler.EditLootDistributionData(moonpoolFragmentClassId, BiomeType.CragField_Ground, 0.8f, 1);
            LootDistributionHandler.EditLootDistributionData(drillArmFragmentClassId, BiomeType.CragField_Ground, 0.8f, 1);
            LootDistributionHandler.EditLootDistributionData(grappleArmFragmentClassId, BiomeType.CragField_Ground, 0.8f, 1);
            LootDistributionHandler.EditLootDistributionData(rockGrubClassId, BiomeType.CragField_OpenShallow_CreatureOnly, 0.7f, 1);
            LootDistributionHandler.EditLootDistributionData(oculusClassId, BiomeType.CragField_OpenDeep_CreatureOnly, 0.6f, 1);

            //Adding Terrain Patch               WARNING!!! IF YOU LOOK AT THIS CODE IT IS AN OBJECTIVE FACT THAT YOU WILL WANT TO KILL YOURSELF. WARNING!!!

            //This..Is probably damn near the ugliest code I've ever written. My eyes burn. But; I can't be fucked figuring out how to make the passes to the method work with a foreach right now,
            //so..Might go back and optimize later, but hey, "At least I didn't do it in Update() :D". And to anyone looking at this code who has a shred of decency..I'm sorry.
            var cragTerrainPatch = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch.optoctreepatch");
            var cragTerrainPatch2 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch2.optoctreepatch");
            var cragTerrainPatch3 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch3.optoctreepatch");
            var cragTerrainPatch4 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch4.optoctreepatch");
            var cragTerrainPatch5 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch5.optoctreepatch");
            var cragTerrainPatch6 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch6.optoctreepatch");
            var cragTerrainPatch7 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch7.optoctreepatch");
            var cragTerrainPatch8 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch8.optoctreepatch");
            var cragTerrainPatch9 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch9.optoctreepatch");
            var cragTerrainPatch10 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch10.optoctreepatch");
            var cragTerrainPatch11 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch11.optoctreepatch");
            var cragTerrainPatch12 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch12.optoctreepatch");
            var cragTerrainPatch13 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch13.optoctreepatch");
            var cragTerrainPatch14 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch14.optoctreepatch");
            var cragTerrainPatch15 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch15.optoctreepatch");
            var cragTerrainPatch16 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch16.optoctreepatch");
            var cragTerrainPatch17 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch17.optoctreepatch");
            var cragTerrainPatch18 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch18.optoctreepatch");
            var cragTerrainPatch19 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch19.optoctreepatch");
            var cragTerrainPatch20 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch20.optoctreepatch");
            var cragTerrainPatch21 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch21.optoctreepatch");
            var cragTerrainPatch22 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch22.optoctreepatch");
            var cragTerrainPatch23 = assembly.GetManifestResourceStream("IncreaseReaperDamage.CragFieldTerrainPatch23.optoctreepatch");

            //JESUS MAN WHY DIDN'T YOU JUST MAKE ON PATCH FOR THE CRAG FIELD?? ..Because I discovered too late that opening different batches doesn't erase progress in the other ones..
            //And..It's too late to go back now..I'd have to start over and..I'm so, sorry..Please forgive me coding gods, for I have commited a horrible sin against all of humanity.

            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch2);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch3);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch4);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch5);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch6);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch7);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch8);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch9);     //It just keeps getting worse..I'm a broken man now..
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch10);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch11);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch12);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch13);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch14);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch15);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch16);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch17);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch18);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch19);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch20);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch21);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch22);
            TerrainPatcher.TerrainRegistry.PatchTerrain(cragTerrainPatch23);

            //ENHANCED CRAGFIELD END

            //TESTCUBE CODE START

            GameObject myAsset;

            string modPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(modPath, "assets/testcubebundle"));

            if (myLoadedAssetBundle == null)
            {
                Logger.Log(Logger.Level.Error, "Failed to load AssetBundle!");
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
            public float damageMultiplier = 1.0f;

            //Slider for the multiple at which to increase the reaper's cyclops damage output
            [Slider("Cyclops Damage Multiplier", Format = "{0:F2}", Min = 1.0f, Max = 5.0f, DefaultValue = 2.0f, Step = 0.1f)]
            public float cyclopsDamageMultiplier = 1.0f;

            //Slider for the multiple at which to increase the reaper's seamoth & exosuit damage output
            [Slider("Seamoth & PRAWN Damage Multiplier", Format = "{0:F2}", Min = 1.0f, Max = 5.0f, DefaultValue = 2.0f, Step = 0.1f)]
            public float seamothDamageMultiplier = 1.0f;
		}

    }
}