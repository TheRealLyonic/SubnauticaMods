using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.PreviewHandler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser.Assets
{
    public class MatAsset : MonoBehaviour
    {
        public PreviewObjectHandler previewObjectHandler;
        
        [SerializeField]
        private Image matPreviewImage;
        
        [SerializeField]
        private TextMeshProUGUI matNameText;

        [SerializeField]
        private Material material;

        public void UpdatePreview(Material previewMat, Texture2D previewImage)
        {
            material = previewMat;
            matNameText.text = Utility.MaterialDatabase.FilterInstanceFromMatName(previewMat.name);
            
            matPreviewImage.sprite = Sprite.Create(previewImage, new Rect(0, 0, previewImage.width, previewImage.height), Vector2.zero);
        }
        
        public void DragMaterial()
        {
            previewObjectHandler.UpdateHoveredObjectMaterial(material);
        }

        public void ApplyMaterial()
        {
            previewObjectHandler.UpdateLastAppliedObjectMat(material);
        }

        public void DropMaterial()
        {
            previewObjectHandler.LockHoveredObjectMaterial();
        }
    }
}