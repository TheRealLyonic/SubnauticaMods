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
            
            CoroutineHost.StartCoroutine(SpawnPlayer());

            return false;
        }

        private static IEnumerator SpawnPlayer()
        {
            var sceneLoading = GameObject.FindObjectOfType<uGUI_SceneLoading>();

            yield return !sceneLoading.isLoading;
            
            Plugin.Logger.LogWarning("Initializing black panel...");
            
            InitBlackPanel();
            
            yield return LargeWorldStreamer.main.IsWorldSettled();

            Player.main.oxygenMgr.AddOxygen(45f);
            
            yield return new WaitForSeconds(10f);

            Player.main.SetPosition(PlayerPatch.SPAWN_POS);

            Player.main.cinematicModeActive = false;

            playerSpawned = true;

            blackUIPanel.SetActive(false);
        }

        private static void InitBlackPanel()
        {
            blackUIPanel = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BlackPanel"));
            
            PrefabUtils.AddBasicComponents(blackUIPanel, "BlackUIPanel", TechType.None, LargeWorldEntity.CellLevel.Near);

            GameObject canvasObject = null;
            foreach(var canvas in GameObject.FindObjectsOfType<uGUI_CanvasScaler>())
            {
                if (canvas.gameObject.name == "ScreenCanvas")
                {
                    canvasObject = canvas.gameObject;
                    break;
                }
            }
            
            blackUIPanel.name = "Lyonic_BlackUIPanel";
            
            if(canvasObject != null)
                blackUIPanel.transform.parent = canvasObject.transform;
            else
                Plugin.Logger.LogWarning("Failed to find the ScreenCanvas object.");
            
            blackUIPanel.transform.position = new Vector3(0f, 0f, 0f);
            blackUIPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
            blackUIPanel.transform.localScale = new Vector3(2000000f, 2000000f, 2000000f);
            
            blackUIPanel.SetActive(true);
        }

    }
}