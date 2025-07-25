using System;
using System.Collections;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono
{
    public class MatPreviewImageGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject previewSphere;

        [SerializeField]
        private GameObject previewBackground;
        
        [SerializeField]
        private Camera matPreviewCamera;

        [SerializeField]
        private Light previewLight;

        private const float BACKWARD_DISTANCE = 3f;
        
        private Transform previewSphereParent;

        private WBOITPreview wboitPreview;

        private void Awake()
        {
            if(previewSphereParent == null)
                previewSphereParent = previewSphere.transform.GetParent();
            
            if(wboitPreview == null)
                wboitPreview = matPreviewCamera.GetComponent<WBOITPreview>();
        }

        public void UpdateImageGenPos(Transform camTransform)
        {
            previewSphereParent.position = camTransform.position + camTransform.forward * -BACKWARD_DISTANCE;
        }

        public  IEnumerator GenerateImage(Material material, TaskResult<Texture2D> imageResult, int width, int height, bool wboitMat)
        {
            //WBOIT Mats refuse to Render if we don't capture them at the current gameResolution for some reason.
            if (!wboitMat)
            {
                width = 256;
                height = 256;
            }
            
            previewSphere.SetActive(true);
            matPreviewCamera.gameObject.SetActive(true);
            previewLight.gameObject.SetActive(true);
            previewBackground.SetActive(true);
            
            previewSphere.GetComponent<MeshRenderer>().material = material;
            
            yield return new WaitForEndOfFrame();

            try
            {
                var renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
                renderTexture.Create();

                matPreviewCamera.targetTexture = renderTexture;

                matPreviewCamera.Render();

                RenderTexture.active = renderTexture;

                var screenCapture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                screenCapture.filterMode = FilterMode.Bilinear;

                screenCapture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                screenCapture.Apply();

                if (screenCapture == null)
                    Plugin.Logger.LogError($"Failed to get preview image for material: {material.name}");

                if (wboitMat)
                {
                    //Rescale the Full-HD image to better fit the dimensions of our TargetGraphic
                    int newWidth = width / 4;
                    int newHeight = height / 4;
                    
                    var tempTex = RenderTexture.GetTemporary(newWidth, newHeight, 0, RenderTextureFormat.ARGB32);
                
                    Graphics.Blit(screenCapture, tempTex);

                    var scaledTexture = new Texture2D(newWidth, newHeight, TextureFormat.ARGB32, false);
                    RenderTexture.active = tempTex;
                
                    scaledTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
                    scaledTexture.Apply();

                    RenderTexture.active = null;
                    RenderTexture.ReleaseTemporary(tempTex);
                
                    imageResult.Set(scaledTexture);
                }else
                    imageResult.Set(screenCapture);

                //Clean up after getting the image
                matPreviewCamera.targetTexture = null;
                RenderTexture.active = null;

                Destroy(renderTexture);
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError("A critical exception occurred while trying to generate a previewImage.");
                Plugin.Logger.LogError("==================================================================================");
                Plugin.Logger.LogError(ex.StackTrace);
                Plugin.Logger.LogError("==================================================================================");
            }
            
            previewLight.gameObject.SetActive(false);
            matPreviewCamera.gameObject.SetActive(false);
            previewSphere.SetActive(false);
            previewBackground.SetActive(false);
        }
        
    }
}