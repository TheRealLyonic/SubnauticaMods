using HarmonyLib;

namespace ImprovedReaper
{
	class IncreaseReaperCyclopsDamage
	{

		[HarmonyPatch(typeof(MeleeAttack))]
		[HarmonyPatch("ManagedUpdate")]

		internal class Patch_MeleeAttack_ManagedUpdate
		{
			[HarmonyPostfix]
			public static void Postfix(MeleeAttack __instance)
			{

				if (__instance.GetType() == typeof(ReaperMeleeAttack))
				{
					ReaperMeleeAttack reaperAttack = __instance as ReaperMeleeAttack;
					float defaultCyclopsDamage = 160f;
					float damageMultiplier = MainPatcher.config.cyclopsDamageMultiplier;

					reaperAttack.cyclopsDamage = defaultCyclopsDamage * damageMultiplier;
				}

			}
		}

	}
}