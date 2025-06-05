using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser
{
    public class FolderAsset : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI folderNameText;

        [SerializeField]
        private string directoryName;

        private uGUI_AssetBrowser assetBrowser;
        private Button button;

        private void Awake()
        {
            if (!assetBrowser)
                assetBrowser = GetComponentInParent<uGUI_AssetBrowser>();
            
            if (!button)
                button = GetComponentInChildren<Button>();
        }

        public void UpdateDirectoryName(string newDirectoryName)
        {
            directoryName = newDirectoryName;
            
            folderNameText.text = directoryName;
            
            button.onClick.AddListener(() => assetBrowser.UpdateDirectory(assetBrowser.currentDirectory + "/" + newDirectoryName));
        }
    }
}