using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(SolarPanel))]
    public class SolarPanelPatch
    {

        [HarmonyPatch(nameof(SolarPanel.OnHandHover))]
        [HarmonyPrefix]
        public static bool OnHandHover_Prefix(SolarPanel __instance)
        {
            if (__instance.gameObject.GetComponent<CustomPowerSource>() != null)
                return false;

            return true;
        }
        
    }
}