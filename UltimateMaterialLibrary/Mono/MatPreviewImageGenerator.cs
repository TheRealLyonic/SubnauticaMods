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

        [SerializeField]
        private Light previewLight;

        private const float BACKWARD_DISTANCE = 3f;
        
        private Transform previewSphereParent;

        private void Awake()
        {
            if(previewSphereParent == null)
                previewSphereParent = previewSphere.transform.GetParent();
        }

        public void UpdateImageGenPos(Transform camTransform)
        {
            previewSphereParent.position = camTransform.position + camTransform.forward * -BACKWARD_DISTANCE;
        }

        public  IEnumerator GenerateImage(Material material, TaskResult<Texture2D> imageResult, int width=256, int height=256)
        {
            previewSphere.SetActive(true);
            matPreviewCamera.gameObject.SetActive(true);
            previewLight.gameObject.SetActive(true);
            
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
            
            previewLight.gameObject.SetActive(false);
            matPreviewCamera.gameObject.SetActive(false);
            previewSphere.SetActive(false);
        }
        
    }
}