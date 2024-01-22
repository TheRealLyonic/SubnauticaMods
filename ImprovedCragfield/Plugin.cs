using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System;
using System.Collections.Generic;
using Nautilus.Handlers;

namespace LyonicDevelopment.ImprovedCragfield
{
	[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
	[BepInDependency("com.snmodding.nautilus")]
	[BepInDependency("Esper89.TerrainPatcher")]
	public class Plugin : BaseUnityPlugin
    {
	    public new static ManualLogSource Logger { get; private set; }
	    
		private const string PLUGIN_GUID = "com.lyonicdevelopment.improvedcragfield";
		private const string PLUGIN_NAME = "Improved Cragfield";
		private const string PLUGIN_VERSION = "1.0.0";

		private static readonly Harmony Harmony = new Harmony(PLUGIN_GUID);

		private void Awake()
		{
			Logger = base.Logger;
			
			CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.ReaperLeviathan, new Vector3(15, -200, -1450)));

			RegisterLoot();

			Harmony.PatchAll();
			
			Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded.");
		}

		private void RegisterLoot()
		{
			List<Tuple<string, BiomeType, float>> lootDistribution = new List<Tuple<string, BiomeType, float>>
			{
				Tuple.Create(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.CragField_Rock, 1.5f),
				Tuple.Create(CraftData.GetClassIdForTechType(TechType.ShaleChunk), BiomeType.CragField_Rock, 0.7f),
				
				Tuple.Create(CraftData.GetClassIdForTechType(TechType.Lithium), BiomeType.CragField_Ground, 0.5f),
				
				Tuple.Create(CraftData.GetClassIdForTechType(TechType.Rockgrub), BiomeType.CragField_OpenDeep_CreatureOnly, 0.8f)
			};

			foreach(var loot in lootDistribution)
			{
				LootDistributionHandler.EditLootDistributionData(loot.Item1, loot.Item2, loot.Item3, 1);
			}
		}
	}
}
