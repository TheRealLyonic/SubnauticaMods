using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class PowerCollider : MonoBehaviour
    {
        public static bool playerInRange = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player")
                playerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.name == "Player")
                playerInRange = false;
        }
        
        //Code for getting rid of the annoying cargo boxes/stasis rifle fragments
        private IEnumerator Start()
        {
            //Give gameobjects time to spawn.
            yield return new WaitForSeconds(4f);
            
            TechTag[] gameObjects = FindObjectsOfType<TechTag>();
            
            List<GameObject> destroyObjects = new List<GameObject>();

            //We should keep track of any objects which are a collection of the cargo-boxes/stasis rifle fragments
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i].type == TechType.StarshipCargoCrate)
                {
                    destroyObjects.Add(gameObjects[i].gameObject.transform.parent.gameObject);
                }
            }

            //Destroy any box/rifle collections which are in range of our spawn location/habitat.
            for (int i = 0; i < destroyObjects.Count; i++)
            {
                DestroyInRange(destroyObjects[i]);
            }
        }

        private void DestroyInRange(GameObject gameObject)
        {
            bool inRangeX = gameObject.transform.position.x <= -787 && gameObject.transform.position.x >= -824;
            bool inRangeY = gameObject.transform.position.y <= 84 && gameObject.transform.position.y >= 63;
            bool inRangeZ = gameObject.transform.position.z <= -1032 && gameObject.transform.position.z >= -1079;

            if (inRangeX || inRangeY || inRangeZ)
            {
                Destroy(gameObject);
            }
        }
    }
}