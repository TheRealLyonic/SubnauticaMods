using System;
using System.Collections;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono
{
    public class MatPreviewImageGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject previewSphere;
        
        [SerializeField]
        private Camera matPreviewCamera;
        
        public IEnumerator GenerateImage(Material material, TaskResult<Texture2D> imageResult, int width=256, int height=256)
        {
            var parentObject = previewSphere.transform.GetParent();
            
            for (int i = 0; i < parentObject.childCount; i++)
            {
                var childObject = parentObject.GetChild(i);
                
                if(childObject.gameObject.name != previewSphere.name && childObject.gameObject.name != matPreviewCamera.gameObject.name)
                    parentObject.GetChild(i).gameObject.SetActive(false);
            }
            
            previewSphere.SetActive(true);
            matPreviewCamera.gameObject.SetActive(true);
            
            yield return new WaitForEndOfFrame();

            try
            {
                previewSphere.GetComponent<MeshRenderer>().material = material;

                var renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

                matPreviewCamera.targetTexture = renderTexture;
                
                matPreviewCamera.Render();
            
                RenderTexture.active = renderTexture;

                var screenCapture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                screenCapture.filterMode = FilterMode.Bilinear;
                
                screenCapture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                screenCapture.Apply();

                if (screenCapture == null)
                    Plugin.Logger.LogError($"Failed to get preview image for material: {material.name}");
            
                imageResult.Set(screenCapture);
            
                //Clean up after getting the image
                matPreviewCamera.targetTexture = null;
                RenderTexture.active = null;

                Destroy(renderTexture);
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError("Critical error occurred when trying to generate material preview image!");
                Plugin.Logger.LogWarning("-----------------------------------------------------------------------------------");
                Plugin.Logger.LogError(ex.StackTrace);
                Plugin.Logger.LogWarning("-----------------------------------------------------------------------------------");
            }
            
            for (int i = 0; i < parentObject.childCount; i++)
            {
                var childObject = parentObject.GetChild(i);
                
                if(childObject.gameObject.name != previewSphere.name && childObject.gameObject.name != matPreviewCamera.gameObject.name)
                    parentObject.GetChild(i).gameObject.SetActive(true);
            }
            
            matPreviewCamera.gameObject.SetActive(false);
            previewSphere.SetActive(false);
        }
        
    }
}