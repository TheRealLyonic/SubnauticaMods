using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn
{
	[HarmonyPatch(typeof(PlayerCinematicController))]
	internal class EscapePodPatch
	{

		private static bool ranOnce = false;

		[HarmonyPatch(nameof(PlayerCinematicController.Start))]
		[HarmonyPostfix]
		public static void Update_Prefix(PlayerCinematicController __instance)
		{
			var player = GameObject.FindObjectOfType<Player>();

			if (player)
			{
				__instance.SkipCinematic(player);
			}
			else
			{
				IslandSpawnPlugin.LOGGER.LogError("PLAYER IS NULL");
			}
			
		}

	}
}
