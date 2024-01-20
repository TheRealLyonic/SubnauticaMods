using HarmonyLib;
using LyonicDevelopment.IslandSpawn.Mono;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(uGUI_MainMenu))]
    public class uGUI_MainMenuPatch
    {

        [HarmonyPatch(nameof(uGUI_MainMenu.StartNewGame))]
        [HarmonyPrefix]
        public static void StartNewGame_Prefix()
        {
            PlayerSpawner.isNewGame = true;
        }
        
    }
}