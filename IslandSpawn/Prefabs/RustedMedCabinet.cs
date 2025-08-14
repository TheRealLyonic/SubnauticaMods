using LyonicDevelopment.IslandSpawn.Core;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using Nautilus.Utility.ThunderkitUtilities;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Prefabs
{
    public class RustedMedCabinet
    {
        private CustomPrefab prefab;
        private PrefabInfo prefabInfo;

        private readonly SpawnLocation spawnLocation = 
            new SpawnLocation(new Vector3(-802.445f, 78.1f, -1051.06f), new Vector3(0f, 285f, 0f));

        public RustedMedCabinet(PrefabInfo prefabInfo)
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
            var model = Plugin.AssetBundle.LoadAsset<GameObject>("RustedMedCabinet.prefab");
            
            model.GetComponent<TechTag>().type = prefabInfo.TechType;
            model.GetComponent<Constructable>().techType = prefabInfo.TechType;
            
            model.GetComponent<SkyApplier>().customSkyPrefab = PrefabRegister.DegasiHabitatSky;
            
            MaterialUtils.ApplySNShaders(model);
            
            var modifications = model.GetAllComponentsInChildren<ApplyMaterialModification>();
            
            foreach(var modifier in modifications)
                modifier.ApplyMaterialModifications();

            foreach (var renderer in model.GetComponentsInChildren<MeshRenderer>())
            {
                foreach (var mat in renderer.materials)
                {
                    if(mat.IsKeywordEnabled("MARMO_SIMPLE_GLASS"))
                        MaterialUtils.SetMaterialTransparent(mat, true);
                }
            }

            return model;
        }
    }
}