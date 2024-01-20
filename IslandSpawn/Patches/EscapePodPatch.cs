using HarmonyLib;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(EscapePod))]
    public class EscapePodPatch
    {

        [HarmonyPatch(nameof(EscapePod.Update))]
        [HarmonyPostfix]
        public static void Update_Postfix(EscapePod __instance)
        {
            if(!GameObject.FindObjectOfType<uGUI_SceneLoading>().isLoading)
                Object.Destroy(__instance.gameObject);
        }
        
    }
}