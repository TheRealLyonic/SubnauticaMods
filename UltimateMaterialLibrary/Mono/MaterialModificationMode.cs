using LyonicDevelopment.UltimateMaterialLibrary.Mono.UI;
using UnityEngine;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono
{
    public class MaterialModificationMode : MonoBehaviour
    {
        public bool sceneLoadingFinished;
        public bool dayMode;
        
        private uGUI_MatEditor matEditor;
        
        private GameObject hudParent;
        private GameObject selectionUIParent;
        
        private void OnEnable()
        {
            if (Plugin.CONFIG.materialModModeEnabled && sceneLoadingFinished)
            {
                SetDay(true);
                
                var umlHudPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("UML_HUD.prefab");

                if (umlHudPrefab == null)
                {
                    Plugin.Logger.LogError("Failed to get UI prefab for MaterialModificationMode!");
                    return;
                }

                hudParent = FindObjectOfType<uGUI_SceneHUD>().transform.gameObject;
                
                selectionUIParent = Instantiate(umlHudPrefab, hudParent.transform.GetChild(0));
                
                matEditor = selectionUIParent.GetComponent<uGUI_MatEditor>();
                
                //Lock player view
                Player.main.playerController.SetEnabled(false);
                MainCameraControl.main.SetEnabled(false);
                
                //Hide UI Elements
                var quickSlotUI = hudParent.GetComponentInChildren<uGUI_QuickSlots>();
                var handReticleUI = hudParent.GetComponentInChildren<HandReticle>();
                
                quickSlotUI.gameObject.SetActive(false);
                handReticleUI.gameObject.SetActive(false);
                
                Inventory.main.quickSlots.DeselectImmediate();
            
                matEditor.EnterSelectionMode();
            
                var depthCompass = hudParent.GetComponentInChildren<uGUI_Compass>().transform.GetParent().gameObject;
                depthCompass.gameObject.SetActive(false);
            }
        }

        public void SetDay(bool isDay)
        {
            dayMode = isDay;
            
            var dayNightCycleInstance = DayNightCycle.main;
            
            if (dayNightCycleInstance != null)
            {
                float addition = isDay ? 600.0f : 0f;

                dayNightCycleInstance.timePassedAsDouble += 1200.0 - dayNightCycleInstance.timePassed % 1200.0 + addition;
                dayNightCycleInstance.UpdateAtmosphere();
                    
                if(dayNightCycleInstance.IsDay() != isDay)
                    dayNightCycleInstance.dayNightCycleChangedEvent.Trigger(isDay);

                dayNightCycleInstance._dayNightSpeed = 0f;
                dayNightCycleInstance.skipTimeMode = false;
            }
            else
                Plugin.Logger.LogError("Failed to get DayNightCycle instance!");
        }
        
    }
}