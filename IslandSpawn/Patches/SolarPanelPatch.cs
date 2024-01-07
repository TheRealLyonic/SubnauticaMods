using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(SolarPanel))]
    public class SolarPanelPatch
    {

        [HarmonyPatch(nameof(SolarPanel.Update))]
        [HarmonyPrefix]
        private static bool Update(SolarPanel __instance)
        {
            if (__instance.gameObject.GetComponent<CustomPowerSource>() != null)
            {
                float amount = __instance.GetRechargeScalar() * DayNightCycle.main.deltaTime * 0.25f * 5f;
                
                __instance.relay.ModifyPower(amount, out float num);
                
                return false;
            }
            else
            {
                return true;
            }
        }

        [HarmonyPatch(nameof(SolarPanel.OnHandHover))]
        [HarmonyPrefix]
        public static bool OnHandHover(SolarPanel __instance)
        {
            if (__instance.gameObject.GetComponent<CustomPowerSource>() != null)
            {
                return false;
            }

            return true;
        }
        
    }
}