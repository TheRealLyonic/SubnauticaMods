using UnityEngine;
using UnityEngine.EventSystems;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.AssetBrowser.Assets
{
    public class DraggableMat : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Texture2D dragCursor;
        
        [SerializeField]
        private MatAsset matAsset;

        private void OnValidate()
        {
            if (matAsset == null)
                matAsset = GetComponent<MatAsset>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Cursor.SetCursor(dragCursor, CursorMode.Auto);
        }

        public void OnDrag(PointerEventData eventData)
        {
            matAsset.DragMaterial();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            matAsset.DropMaterial();
            Cursor.SetCursor(null, CursorMode.Auto);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
        
    }
}