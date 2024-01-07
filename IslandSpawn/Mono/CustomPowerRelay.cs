using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class CustomPowerRelay : PowerRelay
    {
        
        protected new virtual void UpdatePowerState()
        {
            if (!dontConnectToRelays)
                dontConnectToRelays = true;

            if (isDirty)
                isDirty = false;
            
            GameObject customSolarPanel = GameObject.Find("CustomSolarPanel(Clone)");

            if (customSolarPanel != null)
            {
                if (inboundPowerSources.Count == 0)
                {
                    inboundPowerSources.Capacity = 1;
                    inboundPowerSources.Add(customSolarPanel.GetComponent<CustomPowerSource>());
                    Plugin.Logger.LogInfo("Added power source.");
                }
                
                if (!isPowered)
                {
                    //Power up
                    electronicsDisabled = false;
                    isPowered = true;
                    lastPowered = true;
                
                    powerStatus = PowerSystem.Status.Normal;
                    powerUpEvent.Trigger(this);
                }
            }
            else
            {
                Plugin.Logger.LogError("Custom fabricator could not find solar panel.");

                if (isPowered)
                {
                    //Power down
                    electronicsDisabled = true;
                    isPowered = false;
                    lastPowered = false;

                    powerStatus = PowerSystem.Status.Offline;
                    powerDownEvent.Trigger(this);
                }
            }
        }
        
    }
}