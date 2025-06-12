using System.Collections;
using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser;
using Nautilus.Utility;
using UnityEngine;
using Random = System.Random;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI
{
    public class uGUI_MatEditor : MonoBehaviour
    {
        public FreecamController camController { get; private set; }
        
        [SerializeField]
        private GameObject biomeSelectionPrefab;
        
        [SerializeField]
        private GameObject previewParentPrefab;

        [SerializeField]
        private GameObject assetBrowserPrefab;
        
        private MaterialModificationMode modificationMode;
        private uGUI_SceneHUD sceneHUD;

        private GameObject biomeSelectionUI;
        private GameObject previewParent;
        private GameObject assetBrowserObject;

        private uGUI_AssetBrowser assetBrowser;
        
        private GameObject previewObject;

        private SkyApplier previewObjectSky;

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

            previewParent = Instantiate(previewParentPrefab, camController.tr);
            previewParent.transform.localPosition = new Vector3(0f, 0f, camController.tr.forward.z * 5f);

            previewObjectSky = previewParent.GetComponent<SkyApplier>();
        }

        public void ExitSelectionMode()
        {
            UWE.Utils.lockCursor = true;
            
            biomeSelectionUI.SetActive(false);
            
            previewParent.transform.SetParent(null);

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
            
            assetBrowser.previewImageGenerator = previewParent.GetComponent<MatPreviewImageGenerator>();
            
            assetBrowserObject.SetActive(true);
            
            assetBrowser.UpdateDirectory("Assets/Materials");
        }

        public void UpdatePreviewObject()
        {
            previewParent.transform.localPosition = new Vector3(0f, 0f, camController.tr.forward.z * 5f);
            
            if (previewObject == null)
                StartCoroutine(SpawnPrimitiveShape(PrimitiveType.Sphere));
            
            previewObjectSky.UpdateSkyIfNecessary();
        }

        private IEnumerator SpawnPrimitiveShape(PrimitiveType primitiveType)
        {
            previewObject = GameObject.CreatePrimitive(primitiveType);

            previewObject.name = "PreviewObject";
            previewObject.transform.SetParent(previewParent.transform, false);

            var renderers = previewObject.GetComponentsInChildren<Renderer>();

            previewParent.GetComponent<SkyApplier>().renderers = renderers;
            
            var task = new TaskResult<Material>();
            yield return Utility.MaterialDatabase.TryGetMatFromDatabase(new Random().Next(2837), task);
            
            renderers[0].material = task.value;
            
            previewObject.SetActive(true);
        }
        
    }
}