using HarmonyLib;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(uGUI_SceneIntro))]
    public class SceneIntroPatch
    {

        public static bool playerSpawned { get; private set; } = false;

        [HarmonyPatch(nameof(uGUI_SceneIntro.IntroSequence))]
        [HarmonyPrefix]
        public static bool IntroSequence_Prefix(uGUI_SceneIntro __instance)
        {
            __instance.Stop(true);
            
            Player.main.SetPosition(PlayerPatch.SPAWN_POS);

            playerSpawned = true;

            return false;
        }

    }
}