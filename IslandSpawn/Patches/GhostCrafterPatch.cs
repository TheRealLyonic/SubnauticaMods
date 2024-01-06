using HarmonyLib;

namespace LyonicDevelopment.IslandSpawn
{
    
    [HarmonyPatch(typeof(GhostCrafter))]
    public class GhostCrafterPatch
    {

        [HarmonyPatch(nameof(GhostCrafter.Start))]
        [HarmonyPrefix]
        public static void Start_Prefix(GhostCrafter __instance)
        {
            __instance.gameObject.GetComponent<Fabricator>().powerRelay =
                __instance.gameObject.EnsureComponent<CustomPowerRelay>();

            __instance.gameObject.GetComponent<Fabricator>().isDeconstructionObstacle = false;
        }

        [HarmonyPatch(nameof(GhostCrafter.HasEnoughPower))]
        [HarmonyPrefix]
        public static bool HasEnoughPower_Prefix(GhostCrafter __instance, ref bool __result)
        {
            if (__instance.gameObject.name.Contains("customFabricator"))
            {
                __result = true;
                return false;
            }

            return true;
        }
        
    }
}