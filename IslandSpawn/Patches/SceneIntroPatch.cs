using HarmonyLib;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(uGUI_SceneIntro))]
    public class SceneIntroPatch
    {

        [HarmonyPatch(nameof(uGUI_SceneIntro.Play))]
        [HarmonyPrefix]
        public static bool Play(uGUI_SceneIntro __instance)
        {
            MainMenuMusic.Stop();
            return false;
        }
        
    }
}