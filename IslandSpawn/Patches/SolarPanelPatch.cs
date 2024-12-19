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

        [HarmonyPatch(nameof(SolarPanel.Update))]
        [HarmonyPrefix]
        public static bool Update_Prefix(SolarPanel __instance)
        {
            if (__instance.gameObject.GetComponent<CustomPowerSource>() != null)
            {
                float addedPower = __instance.GetRechargeScalar() * DayNightCycle.main.deltaTime * 0.25f * 5f;
                __instance.relay.ModifyPower(addedPower, out _);
                return false;
            }

            return true;
        }
        
    }
}