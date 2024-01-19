using Nautilus.Handlers;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Core
{
    public static class StoryManager
    {
        private static string islandScanDesc = "Short range scans reveal multiple tunnel-systems running through the interior of this island. There appear to be small,"
            + " underwater access points located below the island's beaches. Whether these tunnels were made naturally, or artificially, is unclear.";
        
        public static void RegisterStoryEvents()
        {
            RegisterBiomeEntry("islandScan", "Island Scan Data", islandScanDesc, "FloatingIsland", 
                10f, Plugin.AssetBundle.LoadAsset<Texture2D>("island_databank_hint"));
        }

        private static void RegisterBiomeEntry(string key, string entryTitle, string entryDesc, string biomeName, float stayDuration, Texture2D entryImage=null)
        {
            PDAHandler.AddEncyclopediaEntry(key, "PlanetaryGeology", entryTitle, entryDesc, entryImage);
            StoryGoalHandler.RegisterBiomeGoal(key, Story.GoalType.Encyclopedia, biomeName, stayDuration);
        }
        
    }
}