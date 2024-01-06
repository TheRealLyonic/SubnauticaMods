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
            if(SceneIntroPatch.playerSpawned)
                Object.Destroy(__instance.gameObject);
        }
        
    }
}