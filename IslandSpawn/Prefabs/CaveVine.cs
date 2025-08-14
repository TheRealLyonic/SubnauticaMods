using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Prefabs
{
    public class CaveVine
    {
        private CustomPrefab prefab;
        private PrefabInfo prefabInfo;

        private readonly SpawnLocation spawnLocation = new SpawnLocation(new Vector3(-795.4f, -2f, -1007.3f), new Vector3(0f, 343.19f, 0f));

        public CaveVine(PrefabInfo prefabInfo)
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
            var model = Plugin.AssetBundle.LoadAsset<GameObject>("Seed_Sack.prefab");
            
            //TODO: Replace this with a UML call, adding the vanilla creepvine mats onto this custom object.
            MaterialUtils.ApplySNShaders(model);

            return model;
        }
    }
}