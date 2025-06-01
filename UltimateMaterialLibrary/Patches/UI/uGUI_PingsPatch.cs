using HarmonyLib;

namespace LyonicDevelopment.UltimateMaterialLibrary.Patches
{
    [HarmonyPatch(typeof(uGUI_Pings))]
    public class uGUI_PingsPatch
    {

        [HarmonyPatch(nameof(uGUI_Pings.OnAdd))]
        [HarmonyPrefix]
        public static bool OnAdd_Prefix()
        {
            return !Plugin.CONFIG.materialModModeEnabled;
        }
        
    }
}