using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser
{
    public class MatAsset : MonoBehaviour
    {
        [SerializeField]
        private MatPreviewImageGenerator previewImageGenerator;
        
        [SerializeField]
        private Image matPreviewImage;
        
        [SerializeField]
        private TextMeshProUGUI matNameText;

        [SerializeField]
        private Material material;

        public void UpdatePreviewMaterial(Material mat)
        {
            material = mat;
            
            matNameText.text = mat.name;
            
            StartCoroutine(GetPreviewImageForMaterial());
        }

        private IEnumerator GetPreviewImageForMaterial()
        {
            if (material == null)
            {
                Plugin.Logger.LogError($"Trying to get preview image for null material!");
                yield break;
            }
            
            var taskResult = new TaskResult<Texture2D>();
            yield return previewImageGenerator.GenerateImage(material, taskResult);

            var previewImage = taskResult.value;
            
            matPreviewImage.sprite = Sprite.Create(previewImage, new Rect(0, 0, previewImage.width, previewImage.height), new Vector2(0.5f, 0.5f));;
        }
    }
}