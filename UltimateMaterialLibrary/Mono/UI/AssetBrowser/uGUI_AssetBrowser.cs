using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser
{
    public class uGUI_AssetBrowser : MonoBehaviour
    {
        public string currentDirectory { get; private set; } = "Assets/Materials";
        
        [SerializeField]
        private Transform contentParent;
        
        [SerializeField]
        private TextMeshProUGUI currentPathText;
        
        [SerializeField]
        private List<FolderAsset> currentFolderAssets = new List<FolderAsset>();
        
        [SerializeField]
        private List<MatAsset> currentMaterialAssets = new List<MatAsset>();

        public void UpdateDirectory(string newDirectory)
        {
            for (int i = 0; i < contentParent.childCount; i++)
                Destroy(contentParent.GetChild(i).gameObject);
            
            currentFolderAssets.Clear();
            currentMaterialAssets.Clear();
            
            StartCoroutine(UpdateDirectoryAsync(newDirectory));
        }

        private IEnumerator UpdateDirectoryAsync(string newDirectory)
        {
            currentDirectory = newDirectory;
            currentPathText.text = currentDirectory.Replace("/", " > ");

            var folderNames = Utility.MaterialDatabase.GetAllFoldersInsideDirectory(newDirectory);

            foreach (var folderName in folderNames)
            {
                var folderAssetObject = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("FolderAsset.prefab"), contentParent);

                folderAssetObject.name = folderName;

                var folderAsset = folderAssetObject.GetComponent<FolderAsset>();
                
                folderAsset.UpdateDirectoryName(folderName);
            }

            var taskResult = new TaskResult<List<Material>>();
            yield return Utility.MaterialDatabase.GetAllMaterialsInsideDirectory(newDirectory, taskResult);

            var foundMats = taskResult.value;

            foreach (var material in foundMats)
            {
                var matAssetObject = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("MatAsset.prefab"), contentParent);
                
                var matAsset = matAssetObject.GetComponent<MatAsset>();
                
                matAsset.UpdatePreviewMaterial(material);
            }
        }
    }
}