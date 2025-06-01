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

    }
}