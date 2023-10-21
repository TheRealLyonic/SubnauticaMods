using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using SMLHelper.V2.Handlers;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace ImprovedCragfield
{
	[BepInPlugin(myGUID, pluginName, versionString)]
	[BepInDependency("Esper89.TerrainPatcher")]
	public class MainPatcher : BaseUnityPlugin
    {
		private const string myGUID = "com.lyonic.improvedcragfieldmod";
		private const string pluginName = "Improved Cragfield";
		private const string versionString = "1.0.0";

		private static readonly Harmony harmony = new Harmony(myGUID);

		public static ManualLogSource logger;

		private void Awake()
		{
			//Adds the new reaper spawn
			CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.ReaperLeviathan, new Vector3(15, -200, -1450)));

			//Loot Distribution
			IDictionary<string, Tuple<float, BiomeType>> addedLoot = new Dictionary<string, Tuple<float, BiomeType>>
			{
				//Group 1 - Spawns on the rock
                { CraftData.GetClassIdForTechType(TechType.LimestoneChunk), Tuple.Create(1.5f, BiomeType.CragField_Rock) },
				{ CraftData.GetClassIdForTechType(TechType.ShaleChunk), Tuple.Create(0.7f, BiomeType.CragField_Rock) },
                //Group 2 - Spawns on the ground
				{ CraftData.GetClassIdForTechType(TechType.Lithium), Tuple.Create(0.5f, BiomeType.CragField_Ground) },
                //Group 3 - Spawns in creature-only zone
                { CraftData.GetClassIdForTechType(TechType.Rockgrub), Tuple.Create(0.8f, BiomeType.CragField_OpenDeep_CreatureOnly) }
			};

			foreach(var loot in addedLoot)
			{
				LootDistributionHandler.EditLootDistributionData(loot.Key, loot.Value.Item2, loot.Value.Item1, 1);
			}

			//Adding Terrain Patch
			var cragFieldTerrainPatch = Assembly.GetExecutingAssembly().GetManifestResourceStream("ImprovedCragfield.CragfieldTerrainPatch.optoctreepatch");

			if(cragFieldTerrainPatch == null)
			{
				Logger.LogError("The cragfield terrain patch is null! Check your embedded resource!");
			}

			TerrainPatcher.TerrainRegistry.PatchTerrain("CragfieldTerrainPatch.optoctreepatch", cragFieldTerrainPatch);

			harmony.PatchAll();
			Logger.LogInfo(pluginName + " " + versionString + " loaded.");
			logger = Logger;
		}
	}
}
