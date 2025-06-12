using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser
{
    public class MatAsset : MonoBehaviour
    {
        [SerializeField]
        private Image matPreviewImage;
        
        [SerializeField]
        private TextMeshProUGUI matNameText;

        [SerializeField]
        private Material material;

        public void UpdatePreview(Material previewMat, Texture2D previewImage)
        {
            if (!Plugin.FOUND_MATERIALS.Contains(previewMat.name))
            {
                Plugin.FOUND_MATERIALS.Add(previewMat.name);
                Plugin.Logger.LogWarning(Plugin.FOUND_MATERIALS.Count);
            }
            
            material = previewMat;
            matNameText.text = Utility.MaterialDatabase.FilterInstanceFromMatName(previewMat.name);
            
            matPreviewImage.sprite = Sprite.Create(previewImage, new Rect(0, 0, previewImage.width, previewImage.height), Vector2.zero);
        }
    }
}