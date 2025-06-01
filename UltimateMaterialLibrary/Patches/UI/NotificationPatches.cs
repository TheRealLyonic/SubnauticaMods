using HarmonyLib;

namespace LyonicDevelopment.UltimateMaterialLibrary.Patches
{
    [HarmonyPatch(typeof(PDANotification))]
    public class NotificationPatches
    {

        [HarmonyPatch(nameof(PDANotification.Play))]
        [HarmonyPatch(new []{ typeof(object[]) })]
        [HarmonyPrefix]
        public static bool Play_Prefix()
        {
            return !Plugin.CONFIG.materialModModeEnabled;
        }
        
    }

    [HarmonyPatch(typeof(PDALog))]
    public class PDALogPatch
    {
        [HarmonyPatch(nameof(PDALog.Add))]
        [HarmonyPrefix]
        public static bool Add_Prefix()
        {
            return !Plugin.CONFIG.materialModModeEnabled;
        }
    }

    [HarmonyPatch(typeof(VoiceNotification))]
    public class VoiceNotificationPatch
    {
        [HarmonyPatch(nameof(VoiceNotification.GetCanPlay))]
        [HarmonyPrefix]
        public static bool GetCanPlay_Prefix(bool __result)
        {
            __result = !Plugin.CONFIG.materialModModeEnabled;

            return __result;
        }
    }
}