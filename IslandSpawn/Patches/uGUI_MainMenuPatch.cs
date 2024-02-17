using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(uGUI_MainMenu))]
    public class uGUI_MainMenuPatch
    {

        [HarmonyPatch(nameof(uGUI_MainMenu.OnRightSideOpened))]
        [HarmonyPrefix]
        public static void OnRightSideOpened_Prefix()
        {
            //Fixes for if a user starts a new game without closing/reopening the game from the last session.
            PlayerSpawner.isNewGame = false;
        }

        [HarmonyPatch(nameof(uGUI_MainMenu.StartNewGame))]
        [HarmonyPrefix]
        public static void StartNewGame_Prefix()
        {
            PlayerSpawner.isNewGame = true;
        }
        
    }
}