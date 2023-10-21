using HarmonyLib;

namespace LyonicDevelopment.HeatBladeWarmthBZ
{
	[HarmonyPatch(typeof(PlayerTool))]
	internal class PlayerToolPatch
	{

		[HarmonyPatch(nameof(PlayerTool.animToolName), MethodType.Getter)]
		[HarmonyPrefix]
		public static bool Prefix_animToolName(PlayerTool __instance, ref string __result)
		{
			if(__instance.GetType() == typeof(HeatBlade))
			{
				__result = TechType.Knife.AsString(true);
				return false;
			}
			else
			{
				return true;
			}
		}

	}
}
