using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser;
using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.PreviewHandler;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI
{
    public class uGUI_MatEditor : MonoBehaviour
    {
        public FreecamController camController { get; private set; }
        
        [SerializeField]
        private GameObject biomeSelectionPrefab;

        [SerializeField]
        private GameObject assetBrowserPrefab;

        [SerializeField]
        private PreviewObjectHandler previewObjectHandler;
        
        private MaterialModificationMode modificationMode;
        private uGUI_SceneHUD sceneHUD;

        private GameObject biomeSelectionUI;
        private GameObject assetBrowserObject;

        private uGUI_AssetBrowser assetBrowser;

        private void Awake()
        {
            if (!modificationMode)
                modificationMode = Player.main.GetComponent<MaterialModificationMode>();

            if (!sceneHUD)
                sceneHUD = GetComponentInParent<uGUI_SceneHUD>();
            
            if (!camController)
                camController = Player.main.gameObject.GetComponentInChildren<FreecamController>();
        }

        public void EnterSelectionMode()
        {
            camController.ghostMode = true;
            camController.FreecamToggle();
            camController.enabled = false;

            biomeSelectionUI = Instantiate(biomeSelectionPrefab, transform);
            biomeSelectionUI.SetActive(true);

            UWE.Utils.lockCursor = false;

            //TODO: Spawn Primitive here
            previewObjectHandler.SpawnPreview(camController.tr);
        }

        public void ExitSelectionMode()
        {
            UWE.Utils.lockCursor = true;
            
            biomeSelectionUI.SetActive(false);
            
            previewObjectHandler.UnparentFromCam();

            camController.enabled = true;
            
            EnableAssetBrowser();
        }

        public void EnableAssetBrowser()
        {
            camController.enabled = false;
            UWE.Utils.lockCursor = false;

            if (assetBrowserObject == null)
            {
                assetBrowserObject = Instantiate(assetBrowserPrefab, transform);
                assetBrowser = assetBrowserObject.GetComponent<uGUI_AssetBrowser>();
            }

            assetBrowser.previewObjectHandler = previewObjectHandler;
            assetBrowser.previewImageGenerator = previewObjectHandler.PreviewImageGenerator;
            
            assetBrowserObject.SetActive(true);
            
            assetBrowser.UpdateDirectory("Assets/Materials");
        }
        
    }
}