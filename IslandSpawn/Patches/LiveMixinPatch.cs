using HarmonyLib;

namespace LyonicDevelopment.IslandSpawn.Patches
{
    [HarmonyPatch(typeof(LiveMixin))]
    public class RadioPatch
    {
        /*
         * Fixes a NRE that occurs with the RustedRadio within the first few seconds of being enabled, before the
         * LiveMixin's data property can be set. No side-effects if unhandled, just a nasty error log.
         */
        [HarmonyPatch(nameof(LiveMixin.GetHealthFraction))]
        [HarmonyPrefix]
        public static bool GetHealthFraction_Prefix(LiveMixin __instance, ref float __result)
        {
            __result = __instance.health / 100f;
            return __instance.data != null;
        }
    }
}