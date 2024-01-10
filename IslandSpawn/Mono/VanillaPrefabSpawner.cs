using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn
{
    public class VanillaPrefabSpawner : MonoBehaviour
    {
        
        private static List<Tuple<Vector3, Quaternion>> crashSpawns = new List<Tuple<Vector3, Quaternion>>
        {
            Tuple.Create(new Vector3(-660.66f, -11.3f, -1026.17f), new Quaternion(0.43f, 0f, 0f, 0.90f)),
            Tuple.Create(new Vector3(-688.64f, -10f, -993.64f), new Quaternion(0.56f, 0f, 0f, 0.83f)),
            Tuple.Create(new Vector3(-689.87f, -13.12f, -984.8f), new Quaternion(0f, 0f, -0.23f, 0.97f)),
            Tuple.Create(new Vector3(-662.3f, -14.8f, -1012.82f), new Quaternion(0f, 0f, -0.23f, 0.97f)),
            Tuple.Create(new Vector3(-731.8f, -12.49f, -948.19f), new Quaternion(0f, 0f, -0.56f, 0.83f)),
            Tuple.Create(new Vector3(-778.65f, -7.07f, -977.25f), new Quaternion(0.47f, 0f, 0f, 0.88f)),
            Tuple.Create(new Vector3(-830.28f, -14.3f, -1007.15f), new Quaternion(0.56f, 0f, 0f, 0.83f))
        };

        private static List<Tuple<Vector3, Quaternion>> coralSpawns = new List<Tuple<Vector3, Quaternion>>
        {
            Tuple.Create(new Vector3(-735.7f, -29.3f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
            Tuple.Create(new Vector3(-735.7f, -29.6f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
            Tuple.Create(new Vector3(-735.7f, -30.0f, -993f), new Quaternion(0f, -0.87f, 0f, 0.5f)),
            Tuple.Create(new Vector3(-756.7f, -27.9f, -951f), new Quaternion(0f, -0.87f, 0f, 0.5f))
        };
        
        private static List<Tuple<String, String, List<Tuple<Vector3, Quaternion>>>> vanillaItems =
            new List<Tuple<string, string, List<Tuple<Vector3, Quaternion>>>>
            {
                Tuple.Create(CraftData.GetClassIdForTechType(TechType.CrashHome), "Lyonic_CrashHome(Clone)", crashSpawns),
                Tuple.Create("70eb6270-bf5e-4d6a-8182-484ffcfd8de6", "Lyonic_TableCoral(Clone)", coralSpawns)
            };
        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(4f);

            foreach (var vanillaItem in vanillaItems)
            {
                Plugin.Logger.LogInfo($"Spawning {vanillaItem.Item2} instances...");
                
                CoroutineHost.StartCoroutine(RegisterVanillaItem(vanillaItem.Item1, vanillaItem.Item2,
                    vanillaItem.Item3));
            }
        }

        private static IEnumerator RegisterVanillaItem(string classID, string prefabName, List<Tuple<Vector3, Quaternion>> spawnLocations)
        {
            if (GameObject.Find(prefabName) != null)
            {
                Plugin.Logger.LogWarning($"Tried spawning multiple {prefabName} instances...");
                yield break;
            }
            
            var task = PrefabDatabase.GetPrefabAsync(classID);

            yield return task;
            
            task.TryGetPrefab(out var prefab);

            prefab.name = prefabName.Remove(prefabName.Length - 7, 7); //Removes the (Clone) from the string.

            foreach (var spawnLocation in spawnLocations)
            {
                Instantiate(prefab, spawnLocation.Item1, spawnLocation.Item2);
            }
        }
        
    }
}