using HarmonyLib;

namespace ImprovedReaper
{
	class IncreaseReaperSeamothDamage
	{

		[HarmonyPatch(typeof(ReaperLeviathan))]
		[HarmonyPatch("Update")]

		internal class Patch_ReaperLeviathan_Update
		{
			[HarmonyPostfix]
			public static void Postfix(ReaperLeviathan __instance)
			{
				ReaperLeviathan reaper = __instance; //NOTE: The Seamoth and PRAWN use the same damagePerSecond value, so this will affect both vehicles.
				float defaultSeamothDamage = 15f;
				float damageMultiplier = MainPatcher.config.seamothDamageMultiplier;

				reaper.seamothDamagePerSecond = defaultSeamothDamage * damageMultiplier;
			}
		}

	}
}