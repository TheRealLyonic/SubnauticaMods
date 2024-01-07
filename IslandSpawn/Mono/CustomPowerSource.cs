using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class CustomPowerSource : PowerSource
    {
        private const float MAX_POWER = 100f;
        
        private new void Start()
        {
            maxPower = MAX_POWER;
            
            InvokeRepeating("UpdateConnectionCallback", Random.value, 1f);
            
            Plugin.Logger.LogInfo("Starting power source now...");
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
                gameObject.GetComponent<SolarPanel>().relay = connectedRelay;
                connectedRelay.AddInboundPower(this);
                return true;
            }

            Plugin.Logger.LogError("Custom solar panel couldn't find fabricator!");
            connectedRelay = null;
            return false;
        }
        
        public new void SetPower(float newPower)
        {
            power = newPower;
        }

    }
}