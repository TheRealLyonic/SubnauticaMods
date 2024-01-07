using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class CustomPowerSource : PowerSource
    {
        private new void Start()
        {
            maxPower = 1000f;
            
            InvokeRepeating("UpdateConnectionCallback", Random.value, 1f);
            
            Plugin.Logger.LogWarning("Starting power source now...");
        }

        private new void UpdateConnectionCallback()
        {
            UpdateConnection();
        }
        
        public new bool UpdateConnection()
        {
            GameObject customFabricator = GameObject.Find("CustomFabricator(Clone)");
            
            if (customFabricator != null)
            {
                connectedRelay = customFabricator.GetComponent<CustomPowerRelay>();
                SetPower(1000f);
                connectedRelay.AddInboundPower(this);
                return true;
            }

            Plugin.Logger.LogWarning("Custom solar panel couldn't find fabricator!");
            connectedRelay = null;
            return false;
        }
        
        public new void SetPower(float newPower)
        {
            power = newPower;
        }

    }
}