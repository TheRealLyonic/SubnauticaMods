using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class SeedSackController : MonoBehaviour
    {
        private GameObject fruitParentObject, lightObject;
        private int activeSeedObjects, inactiveSeedObjects;
        
        private void Awake()
        {
            fruitParentObject = transform.GetChild(1).gameObject;
            lightObject = transform.GetChild(2).gameObject;
            
            var fruitPlant = gameObject.GetComponent<FruitPlant>();
            
            fruitPlant.Initialize();
            fruitPlant.fruitSpawnEnabled = true;
        }

        private void Update()
        {
            activeSeedObjects = 0;
            inactiveSeedObjects = 0;

            for (int i = 0; i < fruitParentObject.transform.childCount; i++)
            {
                if (fruitParentObject.transform.GetChild(i).gameObject.activeSelf)
                    activeSeedObjects++;
                else
                    inactiveSeedObjects++;
            }
            
            lightObject.SetActive(inactiveSeedObjects != fruitParentObject.transform.childCount);
        }
    }
}