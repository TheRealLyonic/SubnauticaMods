using System;
using System.Collections.Generic;
using Nautilus.Handlers;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Core
{
    public static class DistributionHandler
    {
        //ClassIDs - Note that the CraftData.GetClassIDForTechType() will not work with some or all of these
        private static string beaconFragment = "a50c91eb-f7cf-4fbf-8157-0aa8d444820c";
        private static string acidMushroom = "fc7c1098-13af-417a-8038-0053b65498e5";
        private static string tableCoral = "70eb6270-bf5e-4d6a-8182-484ffcfd8de6";
        
        private static List<Tuple<Vector3, Vector3>> potentialScrapSpawnLocations = new List<Tuple<Vector3, Vector3>>
        {
            Tuple.Create(new Vector3(-736.03f, 35.84f, -1081.63f), new Vector3(0f, 0f, 0f)),
            Tuple.Create(new Vector3(-775.43f, 14.17f, -1110.16f), new Vector3(0f, 0f, 334.46f)),
            Tuple.Create(new Vector3(-730.57f, 37.07f, -1101.94f), new Vector3(0f, 0f, 4f)),
            Tuple.Create(new Vector3(-742.54f, 24.24f, -1104.27f), new Vector3(0f, 0f, 14.96f)),
            Tuple.Create(new Vector3(-726.15f, 44.92f, -1141.43f), new Vector3(0f, 0f, 0f)),
            Tuple.Create(new Vector3(-769.97f, 13.02f, -1105.8f), new Vector3(0f, 0f, 0f)),
            Tuple.Create(new Vector3(-754.56f, 31.13f, -1084.49f), new Vector3(0f, 0f, 0f)),
            Tuple.Create(new Vector3(-724.21f, 38.13f, -1088.56f), new Vector3(0f, 0f, 0f)),
            Tuple.Create(new Vector3(-794.46f, 68.22f, -1057.24f), new Vector3(0f, 0f, 333.32f)),
            Tuple.Create(new Vector3(-709.3f, 65.58f, -1151.96f), new Vector3(13.82f, 0f, 0f)),
            Tuple.Create(new Vector3(-681.4f, 29f, -1080.55f), new Vector3(0f, 0f, 0f))
        };

        private static List<Tuple<Vector3, Vector3>> scrapSpawnLocations = new List<Tuple<Vector3, Vector3>>();
        
        private static List<Tuple<Vector3, Vector3>> potentialLimestoneSpawnLocations = new List<Tuple<Vector3, Vector3>>
        {
            Tuple.Create(new Vector3(-741.8f, 24.5f, -1102.4f), new Vector3(330f, 0f, 0f)),
            Tuple.Create(new Vector3(-730.3f, 38.2f, -1088.65f), new Vector3(170f, 200f, 230f)),
            Tuple.Create(new Vector3(-795.7f, 69f, -1056.6f), new Vector3(0f, 0f, 291.7f)),
            Tuple.Create(new Vector3(-824.9f, -13.7f, -998f), new Vector3(270f, 102.86f, 0f)),
            Tuple.Create(new Vector3(-813.5f, -8.5f, -1005.6f), new Vector3(70f, 0f, 0f)),
            Tuple.Create(new Vector3(-792.7f, -1.9f, -1013.9f), new Vector3(290f, 137.14f, 0f)),
            Tuple.Create(new Vector3(-808.7f, -4.3f, -979.2f), new Vector3(280f, 0f, 0f)),
            Tuple.Create(new Vector3(-802.44f, 76.1f, -1060.62f), new Vector3(34.82f, 346.16f, 317.69f)),
            Tuple.Create(new Vector3(-762.01f, 14.44f, -1106.3f), new Vector3(312.39f, 50f, 335.63f)),
            Tuple.Create(new Vector3(-707.84f, 63.91f, -1164.87f), new Vector3(58.65f, 347.66f, 311.6f))
        };

        private static List<Tuple<Vector3, Vector3>> limestoneSpawnLocations = new List<Tuple<Vector3, Vector3>>();
        
        public static void RegisterDistribution()
        {
            RegisterVanillaDistribution();
            RegisterLootDistribution();
            RegisterRandomScrapMetalDistribution();
            RegisterRandomLimestoneDistribution();
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
                new SpawnInfo(acidMushroom, new Vector3(-804f, -7.4f, -1003f), new Quaternion(-0.68f, 0f, 0f, 0.73f)),
                new SpawnInfo(acidMushroom, new Vector3(-804.54f, -7.4f, -1003.97f), new Vector3(315f, 273.7f, 42.31f)),
                new SpawnInfo(acidMushroom, new Vector3(-804.1f, -7.25f, -1003.7f), new Vector3(279.4f, 308.5f, 0f)),
                
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
                new SpawnInfo(tableCoral, new Vector3(-762f, -44.9f, -1036.8f), new Quaternion(0f, 0f, 0f, 1f)),
            };
            
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawns(vanillaSpawnInfo);
        }
        
        private static void RegisterLootDistribution()
        {
            //Note that none of the floating island biometypes have valid resource spawns except for the two that are inside and outside of the degasi habitats.
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.FloatingIslands_AbandonedBase_Outside, 0.14f, 1);
            LootDistributionHandler.EditLootDistributionData(beaconFragment, BiomeType.FloatingIslands_AbandonedBase_Outside, 0.05f, 1);
            //The sparse reef is just below the floating island; Good place for fragments + Additional loot spawns?
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.SparseReef_Spike, 0.3f, 1);
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.SandstoneChunk), BiomeType.SparseReef_Spike, 0.2f, 1);
            LootDistributionHandler.EditLootDistributionData(CraftData.GetClassIdForTechType(TechType.LimestoneChunk), BiomeType.SparseReef_Wall, 0.2f, 1);
        }

        private static void RegisterRandomScrapMetalDistribution()
        {
            FillRandomPositions(0, 5, 7);

            foreach (var scrapSpawnLocation in scrapSpawnLocations)
            {
                CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.ScrapMetal, scrapSpawnLocation.Item1, scrapSpawnLocation.Item2));
            }
        }

        private static void RegisterRandomLimestoneDistribution()
        {
            FillRandomPositions(1, 6, 9);

            foreach (var limestoneSpawnLocation in limestoneSpawnLocations)
            {
                CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(TechType.LimestoneChunk, limestoneSpawnLocation.Item1, limestoneSpawnLocation.Item2));
            }
        }

        /*
        I'm aware this is a really odd way of doing things, initially this method was much simpler, took the potential
        spawn location list as a parameter and returned a randomized list to be used above. That caused random but frequent
        game crashes, with no errors in the log. My guess is it has something to do with the backend nature of arrays,
        and things that use them behind the hood. Regardless, this works for the time being. Will revisit if needed. 
        */
        private static void FillRandomPositions(int lootType, int minSpawns, int maxSpawns)
        {
            if (lootType == 0)
            {
                //Scrap
                for (int i = 0; i < potentialScrapSpawnLocations.Count; i++)
                {
                    if(UnityEngine.Random.Range(0, 2) == 0)
                        scrapSpawnLocations.Add(potentialScrapSpawnLocations[i]);

                    if (scrapSpawnLocations.Count >= maxSpawns)
                        break;
                }

                foreach (var scrapSpawnLocation in scrapSpawnLocations)
                {
                    potentialScrapSpawnLocations.Remove(scrapSpawnLocation);
                }

                if (scrapSpawnLocations.Count < minSpawns)
                    FillRandomPositions(0, minSpawns, maxSpawns);
            }
            else
            {
                //Limestone
                for (int i = 0; i < potentialLimestoneSpawnLocations.Count; i++)
                {
                    if(UnityEngine.Random.Range(0, 2) == 0)
                        limestoneSpawnLocations.Add(potentialLimestoneSpawnLocations[i]);

                    if (limestoneSpawnLocations.Count >= maxSpawns)
                        break;
                }

                foreach (var limestoneSpawnLocation in limestoneSpawnLocations)
                {
                    potentialLimestoneSpawnLocations.Remove(limestoneSpawnLocation);
                }

                if (limestoneSpawnLocations.Count < minSpawns)
                    FillRandomPositions(1, minSpawns, maxSpawns);
            }
        }
    }
}