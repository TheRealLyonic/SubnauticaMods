using System.Collections;
using System.IO;
using LyonicDevelopment.UltimateMaterialLibrary.UI;
using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.UltimateMaterialLibrary
{
    [Menu("Ultimate Material Library")]
    public class Config : ConfigFile
    {

        public bool materialModModeEnabled;
        
        [Button("Enter Material Preview")]
        public void OpenMaterialPreviewSettings(ButtonClickedEventArgs e)
        {
            var mainMenuGUI = Object.FindObjectOfType<uGUI_MainMenu>();
            
            CoroutineHost.StartCoroutine(mainMenuGUI.StartNewGame(GameMode.Creative));
            
            materialModModeEnabled = true;
        }

        //Have a camera exist somewhere. Have it be perfectly positioned/lit. Have camera take capture
        //on command, when material is passed in as parameter. Material Updated -> Capture taken -> Texture2D returned.
        //No other logic necessary! Just get the texture, and get out. Setup the camera in advance.
        [Button("Take Screenshot")]
        public void TakeScreenShot(ButtonClickedEventArgs e)
        {
            CoroutineHost.StartCoroutine(TakeCapture(uGUI_MatEditScenePreview.matPreviewCamera));
        }

        private static IEnumerator TakeCapture(Camera imageCam, int width=256, int height=256)
        {
            yield return new WaitForEndOfFrame();
            
            imageCam.gameObject.SetActive(true);
            
            var renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

            imageCam.targetTexture = renderTexture;

            var screenCapture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            
            imageCam.Render();

            RenderTexture.active = renderTexture;

            screenCapture.filterMode = FilterMode.Bilinear;
            screenCapture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            
            imageCam.targetTexture = null;
            RenderTexture.active = null;

            Object.Destroy(renderTexture);

            var pngData = screenCapture.EncodeToPNG();

            string filePath = "C:\\Users\\rwbyf\\Downloads\\render.png";
            
            File.WriteAllBytes(filePath, pngData);
            
            imageCam.gameObject.SetActive(false);
        }

    }
}