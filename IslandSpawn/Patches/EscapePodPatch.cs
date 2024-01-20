using System.Collections;
using HarmonyLib;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(EscapePod))]
    public class EscapePodPatch
    {

        [HarmonyPatch(nameof(EscapePod.Update))]
        [HarmonyPostfix]
        public static void Update_Postfix(EscapePod __instance)
        {
            if (!GameObject.FindObjectOfType<uGUI_SceneLoading>().isLoading)
                CoroutineHost.StartCoroutine(DestroyEscapePod(__instance.gameObject));
        }

        private static IEnumerator DestroyEscapePod(GameObject escapePod)
        {
            yield return new WaitUntil(() => escapePod != null);
            
            GameObject.Destroy(escapePod.gameObject);
            
            Plugin.Logger.LogInfo("EscapePod Destroyed.");
        }
        
    }
}