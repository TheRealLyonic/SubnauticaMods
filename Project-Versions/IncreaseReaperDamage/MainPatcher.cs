using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using Logger = QModManager.Utility.Logger;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Handlers;

namespace IncreaseReaperDamage
{
    [QModCore]
    public static class MainPatcher
    {

        internal static Config config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();

        [QModPatch]
        public static void Patch()
		{
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"JDev_{assembly.GetName().Name}");

            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(assembly);
		}

        [Menu("Reaper Damage")]
        public class Config : ConfigFile
		{
            //Slider for the multiple at which to increase the reaper's player damage output
            [Slider("Player Damage Multiplier", Format = "{0:F2}", Min = 1.0f, Max = 5.0f, DefaultValue = 2.0f, Step = 0.1f)]
            public float damageMultiplier = 1.0f;

            //Slider for the multiple at which to increase the reaper's cyclops damage output
            [Slider("Cyclops Damage Multiplier", Format = "{0:F2}", Min = 1.0f, Max = 5.0f, DefaultValue = 2.0f, Step = 0.1f)]
            public float cyclopsDamageMultiplier = 1.0f;

            //Slider for the multiple at which to increase the reaper's seamoth & exosuit damage output
            [Slider("Seamoth & PRAWN Damage Multiplier", Format = "{0:F2}", Min = 1.0f, Max = 5.0f, DefaultValue = 2.0f, Step = 0.1f)]
            public float seamothDamageMultiplier = 1.0f;
		}

    }
}