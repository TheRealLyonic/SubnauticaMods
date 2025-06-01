using System;
using System.Collections;
using System.Collections.Generic;
using LyonicDevelopment.UltimateMaterialLibrary.Mono;
using LyonicDevelopment.UltimateMaterialLibrary.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LyonicDevelopment.UltimateMaterialLibrary.UI
{
    public class uGUI_MatEditScenePreview : MonoBehaviour
    {
        private static readonly Tuple<Vector3, Vector3>[] spawnOptions = new[]
        {
            Tuple.Create(new Vector3(257.8f, -51.7f, 366f), new Vector3(0f, 113.5f, 0f)), //Grassy Plateau (Shallow)
            Tuple.Create(new Vector3(503f, -136f, 313f), new Vector3(8f, 65f, 0f)), //Mushroom Forest (Shallow)
            Tuple.Create(new Vector3(-660f, -365f, -1283f), new Vector3(0f, 250f, 0f)), //Sea Treader's Path (Deep)
            Tuple.Create(new Vector3(968f, -434f, -1433f), new Vector3(0f, 205f, 0f)), //Crash Zone (Deep)
            Tuple.Create(new Vector3(-658f, 26.8f, -1098.8f), new Vector3(15f, 180f, 0f)), //Degasi Island (Land)
            Tuple.Create(new Vector3(445f, -90f, 1174.9f), new Vector3(10f, 275f, 0f)) //Precursor Moonpool (Interior)
        };

        private static readonly Dictionary<int, string> spawnNameDictionary = new Dictionary<int, string>()
        {
            {0, "grassy_plateau_preview"},
            {1, "mushroom_forest_preview"},
            {2, "sea_treaders_path_preview"},
            {3, "crash_zone_preview"},
            {4, "degasi_island_preview"},
            {5, "precursor_moonpool_preview"}
        };
        
        public GameObject biomeSelectionPrefab;
        public GameObject previewParentPrefab;
        
        private MaterialModificationMode modificationMode;
        private uGUI_SceneHUD sceneHUD;
        private FreecamController camController;

        private GameObject biomeSelectionUI;
        private GameObject previewParent;
        private GameObject previewObject;

        private SkyApplier previewObjectSky;
        
        private int currentLocation;

        private int currentMatIndex;

        private void Awake()
        {
            if (modificationMode == null)
                modificationMode = Player.main.GetComponent<MaterialModificationMode>();

            if (sceneHUD == null)
                sceneHUD = GetComponentInParent<uGUI_SceneHUD>();
            
            if (camController == null)
                camController = Player.main.gameObject.GetComponentInChildren<FreecamController>();
        }

        public void EnterSelectionMode()
        {
            camController.ghostMode = true;
            camController.FreecamToggle();
            camController.enabled = false;

            biomeSelectionUI = Instantiate(biomeSelectionPrefab, transform);
            biomeSelectionUI.SetActive(true);
            
            var buttons = biomeSelectionUI.GetComponentsInChildren<Button>();

            for (int i = 0; i < buttons.Length; i++)
            {
                /*
                If 'i' is passed into the method delegation directly, the parameter references the *final* version
                of the i variable, even though AddListener() is never called on a button after the value updates.
                So using i, the argument that both buttons would give to their onClick() calls would be '2'--The final
                value of i, when the loop ends. For this reason, we must assign a constant variable to the *current*
                value of i, and tell each button to use that constant, unmodified value as their argument.
                */
                int movePos = i;
                
                buttons[i].onClick.AddListener(delegate { MoveLocations(movePos); });
            }

            UWE.Utils.lockCursor = false;

            previewParent = Instantiate(previewParentPrefab, camController.tr);
            previewParent.transform.localPosition = new Vector3(0f, 0f, camController.tr.forward.z * 5f);

            previewObjectSky = previewParent.GetComponent<SkyApplier>();
            
            SetPlayerLocation(0);
        }

        public void SetPlayerLocation(int previewLocation)
        {
            if(previewObject == null)
                StartCoroutine(SpawnPrimitiveShape(PrimitiveType.Sphere));
            
            previewObjectSky.UpdateSkyIfNecessary();
            
            currentLocation = previewLocation;

            biomeSelectionUI.GetComponentInChildren<TextMeshProUGUI>().text = Language.main.Get(spawnNameDictionary[currentLocation]);
            
            camController.tr.position = spawnOptions[previewLocation].Item1;
            camController.tr.eulerAngles = spawnOptions[previewLocation].Item2;
        }

        public void MoveLocations(int moveDirection)
        {
            int adjustment = moveDirection == 0 ? -1 : 1;
            
            currentLocation += adjustment;
            
            if (currentLocation < 0)
                currentLocation = spawnOptions.Length - 1;
            else if (currentLocation == spawnOptions.Length)
                currentLocation = 0;
            
            SetPlayerLocation(currentLocation);
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