using HarmonyLib;
using LyonicDevelopment.UltimateMaterialLibrary.Mono;

namespace LyonicDevelopment.UltimateMaterialLibrary.Patches
{
    [HarmonyPatch(typeof(uGUI_SceneLoading))]
    public class uGUI_SceneLoadingPatch
    {

        [HarmonyPatch(nameof(uGUI_SceneLoading.SetProgress))]
        [HarmonyPostfix]
        public static void SetProgress_Postfix(float value)
        {
            if (value >= 0.999 && Plugin.CONFIG.materialModModeEnabled)
            {
                var matMod = Player.main.GetComponentInChildren<MaterialModificationMode>();
                
                if (Player.main != null && !matMod.enabled)
                {
                    Plugin.Logger.LogDebug("Enabling MaterialModificationMode...");

                    matMod.sceneLoadingFinished = true;
                    Player.main.GetComponentInChildren<MaterialModificationMode>().enabled = true;
                }
            }
        }
        
    }
}