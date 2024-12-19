using UnityEngine;
using Random = UnityEngine.Random;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class CustomPowerRelay : PowerRelay
    {
        public static GameObject powerRelayObject;
        public static bool exists;

        public void Awake()
        {
            exists = false;
        }

        public virtual void Start()
        {
            exists = true;
            powerRelayObject = gameObject;
            
            InvokeRepeating("UpdatePowerState", Random.value, 0.5f);
            InvokeRepeating("MonitorCurrentConnection", Random.value, 1f);

            lastCanConnect = CanMakeConnection();

            StartCoroutine(UpdateConnectionAsync());
            constructable = GetComponent<Constructable>();
            
            RegisterRelay(this);
            
            UpdatePowerState();

            if (WaitScreen.IsWaiting)
            {
                lastPowered = isPowered = true;
                powerStatus = PowerSystem.Status.Normal;
            }
        }
        
        protected new virtual void UpdatePowerState()
        {
            if (!dontConnectToRelays)
                dontConnectToRelays = true;

            if (isDirty)
                isDirty = false;
            
            if (CustomPowerSource.exists)
            {
                if (inboundPowerSources.Count == 0)
                {
                    inboundPowerSources.Capacity = 1;
                    inboundPowerSources.Add(CustomPowerSource.powerSourceObject.GetComponent<CustomPowerSource>());
                    Plugin.Logger.LogDebug("Added power source.");
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
                Plugin.Logger.LogDebug("Custom power relay could not connect to power source, retrying...");

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