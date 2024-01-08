using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn
{
    public class AsyncPrefabSpawner : MonoBehaviour
    {
        private static List<Tuple<Vector3, Quaternion>> crashSpawns = new List<Tuple<Vector3, Quaternion>>
        {
            Tuple.Create(new Vector3(-660.66f, -11.3f, -1026.17f), new Quaternion(0.43f, 0f, 0f, 0.90f)),
            Tuple.Create(new Vector3(-688.64f, -10f, -993.64f), new Quaternion(0.56f, 0f, 0f, 0.83f)),
            Tuple.Create(new Vector3(-689.87f, -13.12f, -984.8f), new Quaternion(0f, 0f, -0.23f, 0.97f)),
            Tuple.Create(new Vector3(-662.3f, -14.8f, -1012.82f), new Quaternion(0f, 0f, -0.23f, 0.97f)),
            Tuple.Create(new Vector3(-731.8f, -12.49f, -948.19f), new Quaternion(0f, 0f, -0.56f, 0.83f))
        };
        
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(4f);
            
            CoroutineHost.StartCoroutine(RegisterCrashFish());
        }

        private static IEnumerator RegisterCrashFish()
        {
            //Another check that is sadly necessary...Lest we receive endless crashfish..
            if (GameObject.Find("CrashHome(Clone)") != null)
            {
                Plugin.Logger.LogWarning("Tried spawning multiple crash-home instances...");
                yield break;
            }
            
            var task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(TechType.CrashHome));

            yield return task;

            task.TryGetPrefab(out var prefab);

            foreach (var spawnLocation in crashSpawns)
            {
                Instantiate(prefab, spawnLocation.Item1, spawnLocation.Item2);
            }
        }
        
    }
}