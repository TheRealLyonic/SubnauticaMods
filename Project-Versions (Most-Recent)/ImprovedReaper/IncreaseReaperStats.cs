using HarmonyLib;
using Logger = QModManager.Utility.Logger;

namespace ImprovedReaper
{
	public static class IncreaseReaperStats
	{
		[HarmonyPatch(typeof(ReaperLeviathan))]
		[HarmonyPatch("Update")]

		internal class Patch_Reaper_Leviathan_Update
		{
			[HarmonyPostfix]
			public static void Postfix(ReaperLeviathan __instance)
			{
				ReaperLeviathan reaper = __instance;

				reaper.SetScale(MainPatcher.config.reaperSizeMultiplier);

				reaper.hearingSensitivity = MainPatcher.config.reaperHSMultiplier;
			}
		}
	}
}
