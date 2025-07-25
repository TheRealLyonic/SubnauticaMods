using System;
using System.Collections;
using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.PreviewHandler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser.Assets
{
    public class MatAsset : MonoBehaviour
    {
        public PreviewObjectHandler previewObjectHandler;
        
        public Material material { get; private set; }

        [SerializeField]
        private Image matPreviewImage;
        
        [SerializeField]
        private TextMeshProUGUI matNameText;

        [SerializeField]
        private string materialName;

        public void Initialize(string materialName)
        {
            this.materialName = materialName;

            matNameText.text = Utility.MaterialDatabase.FilterInstanceFromMatName(materialName);
            
            StartCoroutine(FetchMatFromDatabase());
        }

        private IEnumerator FetchMatFromDatabase()
        {
            var matTask = new TaskResult<Material>();
            yield return Utility.MaterialDatabase.TryGetMatFromDatabase(materialName, matTask);
            
            material = matTask.value;
        }
        
        public void UpdatePreviewImage(Texture2D previewTexture)
        {
            //NRE gets thrown here if the current directory is switched while a MatAsset is setting it's image
            //in this method. The object gets destroyed while the sprite is in the middle of it's set-process,
            //leading to an error. We can just account for this with a try-catch.
            try
            {
                matPreviewImage.sprite = Sprite.Create(previewTexture,
                    new Rect(0, 0, previewTexture.width, previewTexture.height), Vector2.zero);
            }
            catch (NullReferenceException)
            {}
        }

        public void OnDestroy()
        {
            Plugin.Logger.LogWarning("Destroyed MatAsset!");
        }

        public void DragMaterial()
        {
            if(material != null)
                previewObjectHandler.UpdateHoveredObjectMaterial(material);
        }

        public void ApplyMaterial()
        {
            if(material != null)
                previewObjectHandler.UpdateLastAppliedObjectMat(material);
        }

        public void DropMaterial()
        {
            if(material != null)
                previewObjectHandler.LockHoveredObjectMaterial();
        }
    }
}