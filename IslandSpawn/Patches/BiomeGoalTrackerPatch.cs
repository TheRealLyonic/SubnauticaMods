using HarmonyLib;
using Story;

namespace LyonicDevelopment.IslandSpawn
{
    [HarmonyPatch(typeof(BiomeGoalTracker))]
    public class BiomeGoalTrackerPatch
    {

        [HarmonyPatch(nameof(BiomeGoalTracker.Start))]
        [HarmonyPrefix]
        public static bool Start_Prefix(BiomeGoalTracker __instance)
        {
            for (int i = 0; i < __instance.goalData.goals.Length; i++)
            {
                BiomeGoal biomeGoal = __instance.goalData.goals[i];

                //Fixes a number of progression issues within the game that come as a result of spawning on the island.
                //I.e. the Aurora wouldn't start the explosion process because the associated biome-goal was never triggered.
                if (biomeGoal.biome == "safeShallows")
                    biomeGoal.biome = "FloatingIsland";
                
                __instance.goals.Add(biomeGoal);
            }
            
            __instance.StartTracking();

            return false;
        }
        
    }
}