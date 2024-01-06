using System.Collections.Generic;

namespace LyonicDevelopment.IslandSpawn
{
    public class CustomPowerRelay : PowerRelay
    {
        public new bool dontConnectToRelays = true;
        public new bool isPowered = true;
        public new List<IPowerInterface> inboundPowerSources = new List<IPowerInterface>();

        public new float GetPower()
        {
            return 200f;
        }

        public new PowerSystem.Status GetPowerStatus()
        {
            return PowerSystem.Status.Normal;
        }

        public new virtual void UpdatePowerState()
        {
            electronicsDisabled = false;
            isPowered = true;
        }

        public new bool UpdateConnection()
        {
            return true;
        }

    }
}