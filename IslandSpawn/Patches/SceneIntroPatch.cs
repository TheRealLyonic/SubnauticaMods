using System.Collections;
using HarmonyLib;
using Nautilus.Utility;
using UnityEngine;
using UWE;
using Object = UnityEngine.Object;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(uGUI_SceneIntro))]
    public class SceneIntroPatch
    {

        public static bool playerSpawned { get; private set; }

        private static GameObject blackUIPanel;

        [HarmonyPatch(nameof(uGUI_SceneIntro.IntroSequence))]
        [HarmonyPrefix]
        public static bool IntroSequence_Prefix(uGUI_SceneIntro __instance)
        {
            __instance.Stop(true);

            Player.main.SetPosition(new Vector3(PlayerPatch.SPAWN_POS.x, PlayerPatch.SPAWN_POS.y + 20f, PlayerPatch.SPAWN_POS.z));
            Player.main.cinematicModeActive = true;
            
            blackUIPanel = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BlackPanel"));

            PrefabUtils.AddBasicComponents(blackUIPanel, "BlackUIPanel", TechType.None, LargeWorldEntity.CellLevel.Near);
            
            blackUIPanel.transform.parent = GameObject.Find("ScreenCanvas").transform;

            blackUIPanel.transform.position = new Vector3(0f, 0f, 0f);
            blackUIPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
            blackUIPanel.transform.localScale = new Vector3(2000000f, 2000000f, 2000000f);
            
            
            blackUIPanel.SetActive(true);
            
            CoroutineHost.StartCoroutine(SpawnPlayer());

            return false;
        }

        private static IEnumerator SpawnPlayer()
        {
            GameObject powerCollider = GameObject.Find("PowerCollider");

            yield return powerCollider;

            yield return LargeWorldStreamer.main.IsWorldSettled();

            Player.main.oxygenMgr.AddOxygen(45f);

            yield return new WaitForSeconds(10f);

            Player.main.SetPosition(PlayerPatch.SPAWN_POS);

            Player.main.cinematicModeActive = false;

            playerSpawned = true;

            blackUIPanel.SetActive(false);
        }

    }
}