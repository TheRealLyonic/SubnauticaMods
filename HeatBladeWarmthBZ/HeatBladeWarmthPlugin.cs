using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;

namespace LyonicDevelopment.HeatBladeWarmthBZ
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class HeatBladeWarmthPlugin : BaseUnityPlugin
    {
        public static ManualLogSource LOGGER;
        public static ModOptions MOD_OPTIONS;

        private const string PLUGIN_GUID = "com.lyonicdevelopment.heatbladewarmth";
        private const string PLUGIN_NAME = "Heat Blade Warmth";
        private const string PLUGIN_VERSION = "1.0.0";

		private static readonly Harmony harmonyInstance = new Harmony(PLUGIN_GUID);

        private void Awake()
        {
            MOD_OPTIONS = OptionsPanelHandler.RegisterModOptions<ModOptions>();

            harmonyInstance.PatchAll();
            
            Logger.LogInfo($"{PLUGIN_NAME} {PLUGIN_VERSION} loaded.");

            LOGGER = Logger;
        }

    }
}
