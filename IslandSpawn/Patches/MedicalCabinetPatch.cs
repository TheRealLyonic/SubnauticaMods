using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono.Prefabs;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(MedicalCabinet))]
    public class MedicalCabinetPatch
    {

        [HarmonyPatch(nameof(MedicalCabinet.OnHandClick))]
        [HarmonyPrefix]
        public static bool OnHandClick_Prefix(MedicalCabinet __instance, GUIHand hand)
        {
            if (__instance is RustedMedicalCabinet rustedCabinet)
            {
                rustedCabinet.OnHandClick(hand);
                return false;
            }

            return true;
        }
        
    }
}