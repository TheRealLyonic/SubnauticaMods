using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class PrefabDestroyer : MonoBehaviour
    {

        public static bool hasRan;
        
        private List<GameObject> destroyObjects = new List<GameObject>();
        
        //Here we define any TechTypes we want destroyed, as well as whether or not they have a parent object that needs destroyed.
        private List<Tuple<TechType, bool>> unwantedTechTypes = new List<Tuple<TechType, bool>>
        {
            Tuple.Create(TechType.StarshipCargoCrate, true)
        };

        private int secondsPassed;
        
        public IEnumerator Start()
        {
            //Give GameObjects time to spawn
            yield return new WaitUntil(() => PlayerSpawner.playerSpawned);

            if (!hasRan)
            {
                hasRan = true;
                
                secondsPassed = 0;
            
                FindDestroyObjects();

                //Destroy any box/rifle collections which are in range of our spawn location/habitat.
                if (destroyObjects.Count == 0)
                    CoroutineHost.StartCoroutine(WaitForSpawns()); //Sometimes spawns take longer than normal.
                else
                    foreach (GameObject destroyObject in destroyObjects)
                        DestroyNearBase(destroyObject);
            }
        }

        private IEnumerator WaitForSpawns()
        { 
            Plugin.Logger.LogInfo("No cargo crates found, checking for late-spawns...");
            
            /*
            This method was added because cargo creates would occasionally take longer than expected to spawn, resulting
            in the initial check missing them, and them being allowed to persist within the scene. This method will check
            for any unwanted prefabs for 20 seconds after the player spawns. We also account for some boxes spawning in the
            habitat area earlier than others, by continuing to check for unwanted prefabs even if we find some. We avoid
            simply destroying the objects in the list at the end of the 20 seconds so that the unwanted prefabs aren't left
            in the game world for the player to see until the 20 seconds is up. This should fix the problem entirely.
            */
            for (int i = 0; i <= 20; i++)
            {
                FindDestroyObjects();

                if (destroyObjects.Count > 0)
                {
                    foreach (var destroyObject in destroyObjects.ToList())
                    {
                        if (DestroyNearBase(destroyObject))
                        {
                            Plugin.Logger.LogInfo("Late-Spawn destroyed.");
                        }
                        
                        destroyObjects.Remove(destroyObject);
                    }
                }
                
                yield return new WaitForSeconds(1);
            }
            
            Plugin.Logger.LogInfo("Late-spawn check ended.");
        }

        private void FindDestroyObjects()
        {
            TechTag[] techTags = FindObjectsOfType<TechTag>();

            for (int i = 0; i < techTags.Length; i++)
            {
                foreach (var techType in unwantedTechTypes)
                {
                    if (techTags[i].type == techType.Item1)
                    {
                        GameObject destroyObject;

                        if (techType.Item2)
                            destroyObject = techTags[i].gameObject.transform.parent.gameObject;
                        else
                            destroyObject = techTags[i].gameObject;
                        
                        destroyObjects.Add(destroyObject);
                    }
                }
            }
        }

        private bool DestroyNearBase(GameObject destroyObject)
        {
            bool inRangeX = destroyObject.transform.position.x <= -787 && destroyObject.transform.position.x >= -824;
            bool inRangeY = destroyObject.transform.position.y <= 84 && destroyObject.transform.position.y >= 63;
            bool inRangeZ = destroyObject.transform.position.z <= -1032 && destroyObject.transform.position.z >= -1079;

            if (inRangeX || inRangeY || inRangeZ)
            {
                Plugin.Logger.LogInfo("Destroyed unwanted prefab.");
                Destroy(destroyObject);
                return true;
            }

            return false;
        }
        
    }
}