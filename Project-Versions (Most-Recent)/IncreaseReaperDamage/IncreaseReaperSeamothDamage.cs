using HarmonyLib;
using Logger = QModManager.Utility.Logger;

namespace IncreaseReaperDamage
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
				ReaperLeviathan reaper = __instance; //NOTE: The cyclops and seamoth use the same damagePerSecond value, so this will affect both vehicles.
				float defaultSeamothDamage = 15f;
				float damageMultiplier = MainPatcher.config.seamothDamageMultiplier;

				float oldReaperSeamothDamage = reaper.seamothDamagePerSecond;
				float newReaperSeamothDamage = defaultSeamothDamage * damageMultiplier;

				reaper.seamothDamagePerSecond = newReaperSeamothDamage;
			}
		}

	}
}