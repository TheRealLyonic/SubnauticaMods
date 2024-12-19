using UnityEngine;
using Random = UnityEngine.Random;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class CustomPowerSource : PowerSource
    {
        public static GameObject powerSourceObject;
        public static bool exists;

        private void Awake()
        {
            exists = false;
        }

        private new void Start()
        {
            powerSourceObject = gameObject;
            exists = true;
            
            InvokeRepeating("UpdateConnectionCallback", Random.value, 1f);
            
            Plugin.Logger.LogDebug("Starting power source now...");
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

            Plugin.Logger.LogDebug("Custom solar panel couldn't find electronics, retrying...");
            
            connectedRelay = null;
            return false;
        }
        
        public new void SetPower(float newPower)
        {
            power = newPower;
        }

    }
}