using System.Collections;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn
{
    public class AsyncPrefabSpawner : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(4f);
            
            CoroutineHost.StartCoroutine(RegisterFirstAidKit());
        }

        private static IEnumerator RegisterFirstAidKit()
        {
            /*
            Why is this check necessary? Don't ask me...At one point during testing, for whatever reason,
            instead of spawning one first aid kit, this code spawned about 4 of them. Couldn't recreate the issue,
            but hopefully, adding this extra check here will fix the problem.
            */
            if (GameObject.Find("DisinfectedWater(Clone)") != null && GameObject.Find("FirstAidKit(Clone)") == null)
            {
                var task = PrefabDatabase.GetPrefabAsync(CraftData.GetClassIdForTechType(TechType.FirstAidKit));

                yield return task;

                task.TryGetPrefab(out var prefab);
                
                Instantiate(prefab, new Vector3(-804.51f, 77.08f, -1055.75f), new Quaternion(0f, 10.41f, 0f, 0f));

                Destroy(GameObject.Find("DisinfectedWater(Clone)"));
            }
            else
            {
                Plugin.Logger.LogWarning("Tried to spawn first aid kit more than once...");
            }
        }
        
    }
}