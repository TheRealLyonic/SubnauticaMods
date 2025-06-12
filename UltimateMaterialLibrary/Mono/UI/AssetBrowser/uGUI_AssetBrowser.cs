using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser
{
    public class uGUI_AssetBrowser : MonoBehaviour
    {
        public string currentDirectory { get; private set; } = "NULL";

        public MatPreviewImageGenerator previewImageGenerator;

        [SerializeField]
        private GameObject pathButtonPrefab;

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
            
            foreach(var button in pathButtons)
                Destroy(button);
            
            for (int i = 0; i < contentParent.childCount; i++)
                Destroy(contentParent.GetChild(i).gameObject);
            
            pathButtons.Clear();
            currentFolderAssets.Clear();
            currentMaterialAssets.Clear();
            
            StartCoroutine(UpdateDirectoryAsync(newDirectory));
        }

        private IEnumerator UpdateDirectoryAsync(string newDirectory)
        {
            currentDirectory = newDirectory;

            var convertedDirectory = newDirectory.Replace("Assets/Materials/", "");
            convertedDirectory = convertedDirectory.Replace("Assets/Materials", "");
            
            var paths = convertedDirectory.Split('/');
            
            var rootDirectory = Instantiate(pathButtonPrefab, pathContentParent);

            rootDirectory.GetComponent<TextMeshProUGUI>().text = "Assets > Materials";
            
            pathButtons.Add(rootDirectory);

            for (int i = 0; i < paths.Length; i++)
            {
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

            var folderNames = Utility.MaterialDatabase.GetAllFoldersInsideDirectory(newDirectory);

            foreach (var folderName in folderNames)
            {
                var folderAssetObject = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FolderAsset.prefab"), contentParent);

                folderAssetObject.name = folderName;

                var folderAsset = folderAssetObject.GetComponent<FolderAsset>();
                
                folderAsset.UpdateDirectoryName(folderName);
                
                currentFolderAssets.Add(folderAsset);
            }

            var taskResult = new TaskResult<List<Material>>();
            yield return Utility.MaterialDatabase.GetAllMaterialsInsideDirectory(newDirectory, taskResult);

            var foundMats = taskResult.value;

            if (foundMats == null)
                yield break;

            foreach (var material in foundMats)
            {
                var matAssetObject = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("MatAsset.prefab"), contentParent);
                
                var matAsset = matAssetObject.GetComponent<MatAsset>();

                var textureResult = new TaskResult<Texture2D>();
                yield return previewImageGenerator.GenerateImage(material, textureResult);

                var generatedPreview = textureResult.value;

                if (!generatedPreview)
                {
                    Plugin.Logger.LogError($"Failed to generate preview for material: {material.name}");
                    yield break;
                }
                
                matAsset.UpdatePreview(material, generatedPreview);
                
                currentMaterialAssets.Add(matAsset);
            }
        }
    }
}