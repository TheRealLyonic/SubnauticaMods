using HarmonyLib;

namespace LyonicDevelopment.UltimateMaterialLibrary.Patches
{
    [HarmonyPatch(typeof(CSVEntitySpawner))]
    public class CSVEntitySpawnerPatch
    {

        [HarmonyPatch(nameof(CSVEntitySpawner.GetPrefabForSlot))]
        [HarmonyPrefix]
        public static bool GetPrefabForSlot_Prefix(EntitySlot.Filler __result)
        {
            __result = default;

            return !Plugin.CONFIG.materialModModeEnabled;
        }
        
    }
}