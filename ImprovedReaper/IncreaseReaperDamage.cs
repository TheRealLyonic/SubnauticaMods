using HarmonyLib;

namespace ImprovedReaper
{
	public static class IncreaseReaperDamage
	{

		[HarmonyPatch(typeof(MeleeAttack))]
		[HarmonyPatch("ManagedUpdate")]

		internal class Patch_MeleeAttack_ManagedUpdate
		{
			[HarmonyPostfix]
			public static void Postfix(MeleeAttack __instance)
			{

				if(__instance.GetType() == typeof(ReaperMeleeAttack))
				{
					ReaperMeleeAttack reaperAttack = __instance as ReaperMeleeAttack;
					float defaultBiteDamage = 80f;
					float damageMultiplier = MainPatcher.config.damageMultiplier;

					reaperAttack.biteDamage = defaultBiteDamage * damageMultiplier;
				}

			}
		}

	}
}