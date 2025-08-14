using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Prefabs
{
    public class RustedStorageLocker
    {
        private CustomPrefab prefab;
        private PrefabInfo prefabInfo;

        private readonly SpawnLocation spawnLocation = new SpawnLocation(new Vector3(-801.7f, 76.39f, -1044.35f), new Vector3(0f, 194f, 0f));

        public RustedStorageLocker(PrefabInfo prefabInfo)
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
            var model = Plugin.AssetBundle.LoadAsset<GameObject>("RustedStorageLocker.prefab");

            MaterialUtils.ApplySNShaders(model);
            
            return model;
        }
    }
}