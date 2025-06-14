using System;
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
            try
            {
                if (PowerCollider.playerInRange)
                {
                    var powerRelay = CustomPowerRelay.powerRelayObject.GetComponent<CustomPowerRelay>();

                    if (powerRelay.inboundPowerSources.Count > 0)
                    {
                        power = Mathf.RoundToInt(powerRelay.GetPower());
                        maxPower = Mathf.RoundToInt(powerRelay.GetMaxPower());
                        status = powerRelay.GetPowerStatus();

                        __result = true;
                        return false;
                    }
                }
            }catch(NullReferenceException){}
            
            power = 0;
            maxPower = 0;
            status = PowerSystem.Status.Offline;
            return true;
        }
        
    }
}