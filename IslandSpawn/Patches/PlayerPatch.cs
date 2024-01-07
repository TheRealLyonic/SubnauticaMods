using HarmonyLib;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(Player))]
    public class PlayerPatch
    {

        public static Vector3 SPAWN_POS
        {
            get { return new Vector3(-793.2f, 81f, -1067.2f); }
        }

        [HarmonyPatch(nameof(Player.GetRespawnPosition))]
        [HarmonyPrefix]
        public static bool GetRespawnPosition_Prefix(Player __instance, ref Vector3 __result)
        {
            //This ensures that the player will still spawn at sea bases and within the cyclops.
            if (__instance.lastValidSub != null && __instance.CheckSubValid(__instance.lastValidSub))
            {
                RespawnPoint respawnPoint = __instance.lastValidSub.gameObject.GetComponentInChildren<RespawnPoint>();

                if (respawnPoint && !respawnPoint.IsInGhostBase())
                {
                    __result = respawnPoint.GetSpawnPosition();
                    
                    return false;
                }
            }

            __result = SPAWN_POS;
            return false;
        }

        [HarmonyPatch(nameof(Player.MovePlayerToRespawnPoint))]
        [HarmonyPrefix]
        public static bool MovePlayerToRespawnPoint_Prefix(Player __instance)
        {
            __instance.SetPosition(__instance.GetRespawnPosition());

            if (__instance.lastValidSub != null && __instance.CheckSubValid(__instance.lastValidSub))
            {
                __instance.SetCurrentSub(__instance.lastValidSub);
            }

            return false;
        }
        
    }
}