using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Prefabs
{
    public class RustedFabricator
    {
        private CustomPrefab prefab;
        private PrefabInfo prefabInfo;
        
        private readonly SpawnLocation spawnLocation = new SpawnLocation(new Vector3(-804.82f, 78.1f, -1051.96f), new Vector3(0f, 106f, 0f));

        public RustedFabricator(PrefabInfo prefabInfo)
        {
            this.prefabInfo = prefabInfo;
        }

        public void Register()
        {
            prefab = new CustomPrefab(prefabInfo);
            
            prefab.SetGameObject(GetPrefab());
            prefab.SetSpawns(spawnLocation);
            
            prefab.Register();
        }

        private GameObject GetPrefab()
        {
            return null;
        }
    }
}