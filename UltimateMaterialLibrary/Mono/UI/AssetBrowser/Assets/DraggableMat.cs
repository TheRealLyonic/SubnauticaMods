using UnityEngine;
using UnityEngine.EventSystems;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser.Assets
{
    public class DraggableMat : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private MatAsset matAsset;

        private void OnValidate()
        {
            if (matAsset == null)
                matAsset = GetComponent<MatAsset>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            matAsset.DragMaterial();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            matAsset.DropMaterial();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
        
    }
}