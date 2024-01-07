using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(uGUI_PowerIndicator))]
    public class uGUI_PowerIndicatorPatch
    {

        [HarmonyPatch(nameof(uGUI_PowerIndicator.IsPowerEnabled))]
        [HarmonyPrefix]
        public static bool IsPowerEnabled(out int power, out int maxPower, out PowerSystem.Status status, ref bool __result)
        {
            if (PowerCollider.playerInRange)
            {
                var powerRelay = GameObject.Find("CustomFabricator(Clone)").GetComponent<CustomPowerRelay>();

                if (powerRelay.inboundPowerSources.Count > 0)
                {
                    power = Mathf.RoundToInt(powerRelay.inboundPowerSources[0].GetPower());
                    maxPower = Mathf.RoundToInt(powerRelay.inboundPowerSources[0].GetMaxPower());
                    status = powerRelay.GetPowerStatus();

                    __result = true;
                    return false;
                }
            }
            
            power = 0;
            maxPower = 0;
            status = PowerSystem.Status.Offline;
            return true;
        }
        
    }
}