using System.Collections;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class PlayerSpawner : MonoBehaviour
    {
        public static bool playerSpawned { get; private set; }
        public static bool isNewGame;

        private uGUI_SceneLoading sceneLoader; 
        private GameObject blackUIPanel;
        private int secondsToLoad = 14;
        
        public void Start()
        {
            blackUIPanel = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BlackUIPanel"));
            
            blackUIPanel.SetActive(false);
            
            DontDestroyOnLoad(blackUIPanel);
            
            sceneLoader = FindObjectOfType<uGUI_SceneLoading>();

            if (isNewGame)
            {
                playerSpawned = false;
                CoroutineHost.StartCoroutine(SpawnPlayerOnIsland());
            }
            else
                playerSpawned = true;
        }

        private IEnumerator SpawnPlayerOnIsland()
        {
            yield return new WaitUntil(() => !sceneLoader.isLoading);
            
            Plugin.Logger.LogDebug("Spawning player on island...");
                
            Player.main.SetPosition(new Vector3(PlayerPatch.SPAWN_POS.x, PlayerPatch.SPAWN_POS.y + 20f, PlayerPatch.SPAWN_POS.z));
            Player.main.cinematicModeActive = true;

            GameObject screenCanvas = null;
            
            foreach (var canvas in FindObjectsOfType<uGUI_CanvasScaler>())
            {
                if (canvas.gameObject.name == "ScreenCanvas")
                {
                    screenCanvas = canvas.gameObject;
                    break;
                }
            }
            
            if(screenCanvas == null)
                Plugin.Logger.LogError("Couldn't find screen canvas.");
            
            blackUIPanel.transform.SetParent(screenCanvas.transform);
            blackUIPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
            blackUIPanel.SetActive(true);
            
            yield return new WaitUntil(() => LargeWorldStreamer.main.IsWorldSettled());

            CoroutineHost.StartCoroutine(CheckBlackScreen(screenCanvas));
            
            Player.main.oxygenMgr.AddOxygen(45f); //Do this to stop the player from dying...No other way to fix this??

            yield return new WaitForSeconds(secondsToLoad);
            
            Player.main.SetPosition(PlayerPatch.SPAWN_POS);

            Player.main.cinematicModeActive = false;

            playerSpawned = true;
                
            blackUIPanel.SetActive(false);
        }

        private IEnumerator CheckBlackScreen(GameObject screenCanvas)
        {
            for (int i = 0; i < secondsToLoad; i++)
            {
                Plugin.Logger.LogDebug("Checking Panel...");
                
                if (blackUIPanel == null)
                {
                    Plugin.Logger.LogWarning("BlackUIPanel failed, trying to fix...");
                
                    blackUIPanel = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("BlackUIPanel"));
                
                    blackUIPanel.transform.SetParent(screenCanvas.transform);
                    blackUIPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
                    blackUIPanel.SetActive(true);
                }
                
                yield return new WaitForSeconds(1f);
            }
        }
        
    }
}