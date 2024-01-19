using System.Collections.Generic;
using Nautilus.Handlers;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Core
{
    public static class DistributionHandler
    {
        //ClassIDs - Note that the CraftData.GetClassIDForTechType() will not work with some or all of these
        private static string beaconFragment = "a50c91eb-f7cf-4fbf-8157-0aa8d444820c";
        private static string gravtrapFragment = "6e4f85c2-ad1d-4d0a-b20c-1158204ee424";
        private static string acidMushroom = "fc7c1098-13af-417a-8038-0053b65498e5";
        private static string tableCoral = "70eb6270-bf5e-4d6a-8182-484ffcfd8de6";

        public static void RegisterDistribution()
        {
            RegisterVanillaDistribution();
            RegisterLootDistribution();
        }

        private static void RegisterVanillaDistribution()
        {
            var vanillaSpawnInfo = new List<SpawnInfo>
            {
                //Crash homes
                new SpawnInfo(TechType.CrashHome, new Vector3(-660.66f, -11.3f, -1026.17f), new Quaternion(0.43f, 0f, 0f, 0.90f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-688.64f, -10f, -993.64f), new Quaternion(0.56f, 0f, 0f, 0.83f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-689.87f, -13.12f, -984.8f), new Quaternion(0f, 0f, -0.23f, 0.97f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-662.3f, -14.8f, -1012.82f), new Quaternion(0f, 0f, -0.23f, 0.97f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-731.8f, -12.49f, -948.19f), new Quaternion(0f, 0f, -0.56f, 0.83f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-778.65f, -7.07f, -977.25f), new Quaternion(0.47f, 0f, 0f, 0.88f)),
                new SpawnInfo(TechType.CrashHome, new Vector3(-830.28f, -14.3f, -1007.15f), new Quaternion(0.56f, 0f, 0f, 0.83f)),
                
                //Fragments
                new SpawnInfo(TechType.SeaglideFragment, new Vector3(-827.46f, -15.65f, -1000.44f), new Quaternion(0f, 0f, 0.01f, 1f)),
                new SpawnInfo(TechType.SeaglideFragment, new Vector3(-771.19f, -8.03f, -964.03f), new Quaternion(0f, 0f, -0.04f, 1f)),
                
                //Acid mushrooms
                new SpawnInfo(acidMushroom, new Vector3(-804.6f, -7.8f, -1002.5f), new Quaternion(0.56f, 0.56f, 0f, -0.83f)),
                new SpawnInfo(acidMushroom, new Vector3(-805.1f, -8f, -1003.25f), new Quaternion(0.56f, 0.56f, 0f, -0.83f)),
                new SpawnInfo(acidMushroom, new Vector3(-805.3f, -8f, -1004.2f), new Quaternion(0.56f, 0.56f, 0f, -0.83f)),
                new SpawnInfo(acidMushroom, new Vector3(-803.9f, -7.12f, -1004.6f), new Quaternion(-0.68f, 0f, 0f, 0.73f)),
                new SpawnInfo(acidMushroom, new Vector3(-803.5f, -7.35f, -1002.15f), new Quaternion(-0.68f, 0f, 0f, 0.73f)),
                
                //TODO: Add creepvine spawns + More titanium & copper sources so seaglide construction is realistic
                
                //Table coral - Batch 1
                new SpawnInfo(tableCoral, new Vector3(-735.7f, -29.3f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
                new SpawnInfo(tableCoral, new Vector3(-735.7f, -29.6f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
                new SpawnInfo(tableCoral, new Vector3(-735.7f, -30.0f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
                //Table coral - Batch 2
                new SpawnInfo(tableCoral, new Vector3(-756f, -27.9f, -951f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                new SpawnInfo(tableCoral, new Vector3(-756f, -28.2f, -951f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                new SpawnInfo(tableCoral, new Vector3(-755.9f, -28.5f, -951f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                //Table coral - Batch 3
                new SpawnInfo(tableCoral, new Vector3(-755.5f, -27.9f, -949.5f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                new SpawnInfo(tableCoral, new Vector3(-755.5f, -28.2f, -949.5f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                new SpawnInfo(tableCoral, new Vector3(-755.4f, -28.5f, -949.5f), new Quaternion(0f, -0.57f, 0f, 0.82f)),
                //Table coral - Batch 4 (Up)
                new SpawnInfo(tableCoral, new Vector3(-859.9f, -39.1f, -1038.6f), new Quaternion(0f, 0.14f, 0f, 0.99f)),
                //Table coral - Batch 4 (Main)
                new SpawnInfo(tableCoral, new Vector3(-860.3f, -40.6f, -1038.6f), new Quaternion(0f, 0.14f, 0f, 0.99f)),
                new SpawnInfo(tableCoral, new Vector3(-860.3f, -40.9f, -1038.6f), new Quaternion(0f, 0.14f, 0f, 0.99f)),
                new SpawnInfo(tableCoral, new Vector3(-860.3f, -41.2f, -1038.65f), new Quaternion(0f, 0.14f, 0f, 0.99f)),
                //Table coral - Batch 5
                new SpawnInfo(tableCoral, new Vector3(-762f, -44.0f, -1036.75f), new Quaternion(0f, 0f, 0f, 1f)),
                new SpawnInfo(tableCoral, new Vector3(-762f, -44.3f, -1036.75f), new Quaternion(0f, 0f, 0f, 1f)),
                new SpawnInfo(tableCoral, new Vector3(-762f, -44.6f, -1036.75f), new Quaternion(0f, 0f, 0f, 1f)),
                new SpawnInfo(tableCoral, new Vector3(-762f, -44.9f, -1036.8f), new Quaternion(0f, 0f, 0f, 1f))
            };
            
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawns(vanillaSpawnInfo);
        }
        
        private static void RegisterLootDistribution()
        {
            //Note that none of the floating island biometypes have valid resource spawns except for the two that are inside and outside of the degasi habitats.
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.FloatingIslands_AbandonedBase_Outside, 0.09f, 1);
            LootDistributionHandler.EditLootDistributionData(beaconFragment, BiomeType.FloatingIslands_AbandonedBase_Outside, 0.08f, 1);
            LootDistributionHandler.EditLootDistributionData(gravtrapFragment, BiomeType.FloatingIslands_AbandonedBase_Outside, 0.07f, 1);
            //The sparse reef is just below the floating island; Good place for fragments + Additional loot spawns?
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.SparseReef_Spike, 0.2f, 1);
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.SandstoneChunk), BiomeType.SparseReef_Spike, 0.1f, 1);
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.SparseReef_Wall, 0.2f, 1);
        }

    }
}