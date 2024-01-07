using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono;

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
                float num;

                __instance.relay.ModifyPower(amount, out num);
                
                return false;
            }
            else
            {
                return true;
            }
        }
        
    }
}