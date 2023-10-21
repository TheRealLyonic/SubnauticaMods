using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyonicDevelopment.IslandSpawn
{
	[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
	public class IslandSpawnPlugin : BaseUnityPlugin
	{
		public static ManualLogSource LOGGER;
		public static ModOptions MOD_OPTIONS;

		private const string PLUGIN_GUID = "com.lyonicdevelopment.islandspawn";
		private const string PLUGIN_NAME = "Island Spawn";
		private const string PLUGIN_VERSION = "1.0.0";

		private static readonly Harmony harmonyInstance = new Harmony(PLUGIN_GUID);

		private void Awake()
		{
			MOD_OPTIONS = OptionsPanelHandler.RegisterModOptions<ModOptions>();

			harmonyInstance.PatchAll();

			Logger.LogInfo($"{PLUGIN_NAME} {PLUGIN_VERSION} loaded.");

			LOGGER = Logger;
		}

	}
}
