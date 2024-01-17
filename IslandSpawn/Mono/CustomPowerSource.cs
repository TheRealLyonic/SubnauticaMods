using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class CustomPowerSource : PowerSource
    {
        public static GameObject powerSourceObject;
        public static bool exists;
        
        private const float MAX_POWER = 100f;
        
        private new void Start()
        {
            maxPower = MAX_POWER;

            powerSourceObject = gameObject;
            exists = true;
            
            InvokeRepeating("UpdateConnectionCallback", Random.value, 1f);
            
            Plugin.Logger.LogInfo("Starting power source now...");
        }

        private new void UpdateConnectionCallback()
        {
            UpdateConnection();
        }
        
        public new bool UpdateConnection()
        {
            if (CustomPowerRelay.exists)
            {
                
                
                connectedRelay = CustomPowerRelay.powerRelayObject.GetComponent<CustomPowerRelay>();
                gameObject.GetComponent<SolarPanel>().relay = connectedRelay;
                connectedRelay.AddInboundPower(this);
                return true;
            }

            Plugin.Logger.LogError("Custom solar panel couldn't find electronics!");
            
            connectedRelay = null;
            return false;
        }
        
        public new void SetPower(float newPower)
        {
            power = newPower;
        }

    }
}