using System;
using System.Collections;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    /*
    The base-game does have a monobehaviour for this already, 'SpawnEscapePodSupplies', but it's code is only called
    when the lifepod registers as a newborn, and seeing as this mod deletes the escape pod, that'll never really happen..
    So without a good method to patch into, I figured this would be the best approach, and it allows me to make the code
    a bit cleaner anyways, compared to what you can find in DNSpy.
    */
    public class AddLifepodEssentials : MonoBehaviour
    {
        public StorageContainer storageContainer;
        
        private void Start()
        {
            if(PlayerSpawner.isNewGame)
                CoroutineHost.StartCoroutine(AddEssentialItems());
        }

        private IEnumerator AddEssentialItems()
        {
            foreach (var essentialItem in LootSpawner.main.GetEscapePodStorageTechTypes())
            {
                CoroutineTask<GameObject> prefabRequest = CraftData.GetPrefabForTechTypeAsync(essentialItem);

                yield return prefabRequest;

                Pickupable pickupable = CraftData.InstantiateFromPrefab(prefabRequest.GetResult(), essentialItem)
                    .GetComponent<Pickupable>();
                
                pickupable.Initialize();

                if (storageContainer.container.HasRoomFor(pickupable)) 
                    storageContainer.container.UnsafeAdd(new InventoryItem(pickupable));
                else 
                    Destroy(pickupable.gameObject);
            }
        }
    }
}