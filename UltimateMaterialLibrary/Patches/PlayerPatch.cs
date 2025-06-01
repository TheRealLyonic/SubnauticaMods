using HarmonyLib;
using LyonicDevelopment.UltimateMaterialLibrary.Mono;

namespace LyonicDevelopment.UltimateMaterialLibrary.Patches
{
    [HarmonyPatch(typeof(Player))]
    public class PlayerPatch
    {

        [HarmonyPatch(nameof(Player.Awake))]
        [HarmonyPostfix]
        public static void Awake_Postfix(Player __instance)
        {
            if (Player.main != null)
                Player.main.gameObject.AddComponent<MaterialModificationMode>().enabled = false;
        }
        
    }
}