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
                    
                    powerUpEvent.Trigger(this);
                }

                int power = Mathf.RoundToInt(inboundPowerSources[0].GetPower());

                if (power > 20)
                    powerStatus = PowerSystem.Status.Normal;
                else if (power > 0)
                    powerStatus = PowerSystem.Status.Emergency;
                else
                    powerStatus = PowerSystem.Status.Offline;
            }
            else
            {
                Plugin.Logger.LogError("Custom power relay could not find solar panel.");

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