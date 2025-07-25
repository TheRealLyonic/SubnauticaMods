using System.Collections;
using System.Collections.Generic;
using BepInEx;
using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser.Assets;
using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.PreviewHandler;
using LyonicDevelopment.UltimateMaterialLibrary.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser
{
    public class uGUI_AssetBrowser : MonoBehaviour
    {
        public PreviewObjectHandler previewObjectHandler;
        public MatPreviewImageGenerator previewImageGenerator;
        
        private string currentDirectory = "NULL";
        
        [SerializeField]
        private GameObject pathButtonPrefab;

        [SerializeField]
        private GameObject folderAssetPrefab;
        
        [SerializeField]
        private GameObject matAssetPrefab;

        [SerializeField]
        private Transform pathContentParent;
        
        [SerializeField]
        private Transform contentParent;
        
        [SerializeField]
        private List<FolderAsset> currentFolderAssets = new List<FolderAsset>();
        
        [SerializeField]
        private List<MatAsset> currentMaterialAssets = new List<MatAsset>();
        
        private List<GameObject> pathButtons = new List<GameObject>();

        public void UpdateDirectory(string newDirectory)
        {
            if (currentDirectory.Equals(newDirectory))
                return;
            
            //Clear the current directory
            StopCoroutine(nameof(GeneratePreviewImages));

            for (int i = 0; i < pathContentParent.childCount; i++)
                Destroy(pathContentParent.GetChild(i).gameObject);

            for (int i = 0; i < contentParent.childCount; i++)
                Destroy(contentParent.GetChild(i).gameObject);
            
            pathButtons.Clear();
            currentFolderAssets.Clear();
            currentMaterialAssets.Clear();
            
            //Update our current set directory info across the board
            currentDirectory = newDirectory;

            var replaceString = "Assets/Materials";
            var convertedDirectory = newDirectory.Substring(replaceString.Length);
            
            if(convertedDirectory.StartsWith("/"))
                convertedDirectory = convertedDirectory.Substring(1);
            
            var paths = convertedDirectory.Split('/');
            
            var rootDirectory = Instantiate(pathButtonPrefab, pathContentParent);

            rootDirectory.GetComponent<TextMeshProUGUI>().text = "Assets > Materials";
            
            pathButtons.Add(rootDirectory);

            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i].IsNullOrWhiteSpace())
                    continue;
                
                var newPath = Instantiate(pathButtonPrefab, pathContentParent);
                
                newPath.GetComponent<TextMeshProUGUI>().text = " > " + paths[i];
                
                pathButtons.Add(newPath);
            }

            for (int i = 0; i < pathButtons.Count; i++)
            {
                var button = pathButtons[i].GetComponent<Button>();

                int currentIndex = i;
                button.onClick.AddListener(() =>
                {
                    string finalPath = "Assets/Materials";
                    
                    for (int j = 0; j < currentIndex; j++)
                        finalPath += "/" + paths[j];
                    
                    UpdateDirectory(finalPath);
                });
            }

            var folderNames = MatDirectoryHandler.GetAllFoldersInsideDirectory(newDirectory);

            foreach (var folderName in folderNames)
            {
                var folderAssetObject = Instantiate(folderAssetPrefab, contentParent);

                folderAssetObject.name = folderName;

                var folderAsset = folderAssetObject.GetComponent<FolderAsset>();
                
                folderAsset.UpdateDirectoryName(folderName);
                
                currentFolderAssets.Add(folderAsset);
            }
            
            var foundMatNames = MatDirectoryHandler.GetAllMaterialsInsideDirectory(newDirectory);

            if (foundMatNames != null)
            {
                foreach (var matName in foundMatNames)
                {
                    var matAssetObject = Instantiate(matAssetPrefab, contentParent);
                
                    var matAsset = matAssetObject.GetComponent<MatAsset>();
                
                    matAsset.previewObjectHandler = previewObjectHandler;
                
                    matAsset.Initialize(matName);
                
                    currentMaterialAssets.Add(matAsset);
                }
                
                StartCoroutine(GeneratePreviewImages());
            }
        }

        public void SearchRecursive(string searchString)
        {
            if (searchString.IsNullOrWhiteSpace())
                return;
            
            StopCoroutine(nameof(GeneratePreviewImages));

            for (int i = 0; i < pathContentParent.childCount; i++)
            {
                var pathObj = pathContentParent.GetChild(i).gameObject;

                if (!pathButtons.Contains(pathObj))
                {
                    currentDirectory = currentDirectory.Substring(0, currentDirectory.LastIndexOf('/'));
                    Destroy(pathObj);
                }
            }
            
            for (int i = 0; i < contentParent.childCount; i++)
                Destroy(contentParent.GetChild(i).gameObject);
            
            currentFolderAssets.Clear();
            currentMaterialAssets.Clear();

            var newPath = Instantiate(pathButtonPrefab, pathContentParent);
            newPath.GetComponent<TextMeshProUGUI>().text = " > \"" + searchString + "\"";
            
            var folderNames = MatDirectoryHandler.GetAllFoldersThatContain(currentDirectory, searchString);

            foreach (var folderName in folderNames)
            {
                var folderAssetObject = Instantiate(folderAssetPrefab, contentParent);
                
                var trimmedName = folderName.Substring(folderName.LastIndexOf('/') + 1);

                folderAssetObject.name = trimmedName;
                
                var folderAsset = folderAssetObject.GetComponent<FolderAsset>();
                
                folderAsset.UpdateDirectoryName(folderName, trimmedName);
                
                currentFolderAssets.Add(folderAsset);
            }
            
            var matNames = MatDirectoryHandler.GetAllMaterialsThatContain(currentDirectory, searchString);

            foreach (var matName in matNames)
            {
                var matAssetObject = Instantiate(matAssetPrefab, contentParent);
                
                var matAsset = matAssetObject.GetComponent<MatAsset>();
                
                matAsset.previewObjectHandler = previewObjectHandler;
                matAsset.Initialize(matName);
                
                currentMaterialAssets.Add(matAsset);
            }
            
            StartCoroutine(GeneratePreviewImages());
            
            currentDirectory = currentDirectory + "/" + searchString;
        }

        public string GetCurrentDirectory()
        {
            for (int i = 0; i < pathContentParent.childCount; i++)
                if (!pathButtons.Contains(pathContentParent.GetChild(i).gameObject))
                    currentDirectory = currentDirectory.Substring(0, currentDirectory.LastIndexOf('/'));
            
            return currentDirectory;
        }

        private IEnumerator GeneratePreviewImages()
        {
            var currentList = currentMaterialAssets.ToArray();
            
            foreach (var matAsset in currentList)
            {
                yield return new WaitUntil(() => matAsset.material != null);

                var task = new TaskResult<Texture2D>();

                var currentResolution = DisplayManager.GetResolution();
                yield return previewImageGenerator.GenerateImage(matAsset.material, task, currentResolution.width, currentResolution.height, matAsset.material.IsKeywordEnabled("WBOIT"));
                
                var generatedPreview = task.value;

                if (!generatedPreview)
                {
                    Plugin.Logger.LogError($"Failed to generate preview image for material: {matAsset.material.name}");
                    yield break;
                }
                
                if (!currentMaterialAssets.Contains(matAsset))
                    yield break;

                matAsset.UpdatePreviewImage(generatedPreview);
            }
        }
    }
}