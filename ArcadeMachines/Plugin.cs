using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LyonicDevelopment.ArcadeMachines
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }

        private const string PLUGIN_GUID = "com.lyonicdevelopment.arcademachines";
        private const string PLUGIN_NAME = "Arcade Machines";
        private const string PLUGIN_VERSION = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(PLUGIN_GUID);

        private void Awake()
        {
            Logger = base.Logger;

            Harmony.PatchAll();

            Logger.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded.");
        }
    }
}