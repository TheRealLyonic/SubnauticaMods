using System.Collections;
using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI
{
    public class uGUI_MatEditor : MonoBehaviour
    {
        public FreecamController camController { get; private set; }
        
        public GameObject biomeSelectionPrefab;
        public GameObject previewParentPrefab;
        
        private MaterialModificationMode modificationMode;
        private uGUI_AssetBrowser assetBrowser;
        private uGUI_SceneHUD sceneHUD;

        private GameObject biomeSelectionUI;
        private GameObject previewParent;
        private GameObject previewObject;

        private SkyApplier previewObjectSky;

        private int currentMatIndex;

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
            
            Destroy(biomeSelectionUI);
            
            previewParent.transform.SetParent(null);

            camController.enabled = true;
            
            EnableAssetBrowser();
        }

        public void EnableAssetBrowser()
        {
            camController.enabled = false;
            UWE.Utils.lockCursor = false;
            
            if (assetBrowser == null)
                assetBrowser = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("AssetBrowser.prefab"), transform).GetComponent<uGUI_AssetBrowser>();
            
            assetBrowser.gameObject.SetActive(true);
            
            assetBrowser.UpdateDirectory("Assets/Materials");
        }

        public void UpdatePreviewObject()
        {
            if (previewObject == null)
                StartCoroutine(SpawnPrimitiveShape(PrimitiveType.Sphere));
            
            previewObjectSky.UpdateSkyIfNecessary();
        }

        public void UpdateMaterial(int moveDirection)
        {
            StartCoroutine(NavigateMaterials(moveDirection));
        }

        private IEnumerator NavigateMaterials(int adjustment)
        {
            currentMatIndex += adjustment;

            if (currentMatIndex > Utility.MaterialDatabase.currentSize - 1)
                currentMatIndex = 0;
            else if (currentMatIndex < 0)
                currentMatIndex = Utility.MaterialDatabase.currentSize - 1;
            
            var task = new TaskResult<Material>();
            yield return Utility.MaterialDatabase.TryGetMatFromDatabase(currentMatIndex, task);
            
            var foundMat = task.value;
            
            Plugin.Logger.LogWarning(foundMat.name);
            
            previewObject.GetComponentInChildren<Renderer>().material = foundMat;
        }

        private IEnumerator SpawnPrimitiveShape(PrimitiveType primitiveType)
        {
            previewObject = GameObject.CreatePrimitive(primitiveType);

            previewObject.name = "PreviewObject";
            previewObject.transform.SetParent(previewParent.transform, false);

            var renderers = previewObject.GetComponentsInChildren<Renderer>();

            previewParent.GetComponent<SkyApplier>().renderers = renderers;
            
            var task = new TaskResult<Material>();
            yield return Utility.MaterialDatabase.TryGetMatFromDatabase(currentMatIndex, task);
            
            renderers[0].material = task.value;
            
            previewObject.SetActive(true);
        }
        
    }
}