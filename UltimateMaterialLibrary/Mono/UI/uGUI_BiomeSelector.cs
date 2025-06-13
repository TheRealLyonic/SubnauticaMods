using System;
using System.Collections.Generic;
using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.PreviewHandler;
using TMPro;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI
{
    public class uGUI_BiomeSelector : MonoBehaviour
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

        [SerializeField]
        private TextMeshProUGUI biomeDisplayText;
        
        private int currentLocation;
        
        private uGUI_MatEditor matEditor;
        private PreviewObjectHandler previewObjectHandler;

        private void Awake()
        {
            if (!matEditor)
                matEditor = GetComponentInParent<uGUI_MatEditor>();
            
            if(!previewObjectHandler)
                previewObjectHandler = GetComponentInParent<PreviewObjectHandler>();
        }

        private void Start()
        {
            UpdatePlayerLocation(0);
        }

        public void MoveLocations(int moveDirection)
        {
            currentLocation += moveDirection;

            if (currentLocation < 0)
                currentLocation = spawnOptions.Length - 1;
            else if (currentLocation == spawnOptions.Length)
                currentLocation = 0;

            UpdatePlayerLocation(currentLocation);
        }

        public void UpdatePlayerLocation(int locationIndex)
        {
            currentLocation = locationIndex;
            
            matEditor.camController.tr.position = spawnOptions[locationIndex].Item1;
            matEditor.camController.tr.eulerAngles = spawnOptions[locationIndex].Item2;
            
            biomeDisplayText.text = Language.main.Get(spawnNameDictionary[locationIndex]);
            
            previewObjectHandler.UpdatePreviewObjectLocation(matEditor.camController.tr);
        }

        public void ConfirmSelection()
        {
            matEditor.ExitSelectionMode();
        }
        
    }
}