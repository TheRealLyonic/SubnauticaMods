using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class SeedSackController : MonoBehaviour
    {
        public GameObject lightObject;
        public FruitPlant fruitPlant;
        
        private int inactiveSeedObjects, activeSeedObjects;
        
        private void Awake()
        {
            fruitPlant.Initialize();
            fruitPlant.fruitSpawnEnabled = true;
        }

        private void Update()
        {
            activeSeedObjects = 0;
            inactiveSeedObjects = 0;
            
            for (int i = 0; i < fruitPlant.fruits.Length; i++)
            {
                if (fruitPlant.fruits[i].gameObject.activeSelf)
                    activeSeedObjects++;
                else
                    inactiveSeedObjects++;
            }
            
            lightObject.SetActive(inactiveSeedObjects != fruitPlant.fruits.Length);
        }
    }
}