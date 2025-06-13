using System;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.PreviewHandler
{
    public class MouseHoverDetector : MonoBehaviour
    {
        private PreviewObjectHandler previewObjectHandler;
        
        public void SetHandler(PreviewObjectHandler previewObjectHandler)
        {
            this.previewObjectHandler = previewObjectHandler;
        }
        
        public void OnMouseOver()
        {
            previewObjectHandler.UpdateHoveredObject(gameObject);
        }

        public void OnMouseExit()
        {
            previewObjectHandler.UpdateHoveredObject(null);
        }
    }
}