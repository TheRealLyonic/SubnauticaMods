using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Prefabs
{
    public class RustedSolarPanel
    {
        private CustomPrefab prefab;
        private PrefabInfo prefabInfo;

        private readonly SpawnLocation[] spawnLocations =
        {
            new SpawnLocation(new Vector3(-804.3f, 79.44f, -1053.95f), new Vector3(0f, 240f, 0f)),
            new SpawnLocation(new Vector3(-802.95f, 79.44f, -1049.05f), new Vector3(0f, 325.714f, 0f))
        };

        public RustedSolarPanel(PrefabInfo prefabInfo)
        {
            this.prefabInfo = prefabInfo;
        }

        public void Register()
        {
            prefab = new CustomPrefab(prefabInfo);

            prefab.SetGameObject(GetPrefab);
            prefab.SetSpawns(spawnLocations);
            
            prefab.Register();
        }

        private IEnumerator GetPrefab(IOut<GameObject> prefab)
        {
            var model = Plugin.AssetBundle.LoadAsset<GameObject>("RustedSolarPanel.prefab");

            model.GetComponent<TechTag>().type = prefabInfo.TechType;

            var solarPanelTask = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel);

            yield return solarPanelTask;

            var vanillaSolarPanel = solarPanelTask.GetResult();
            
            if(!vanillaSolarPanel)
                Plugin.Logger.LogError("Failed to get vanilla solar panel prefab while registering rusted variant.");
            
            model.GetComponent<PowerRelay>().powerSystemPreviewPrefab = vanillaSolarPanel.GetComponent<PowerRelay>().powerSystemPreviewPrefab;
            model.GetComponent<PowerFX>().vfxPrefab = vanillaSolarPanel.GetComponent<PowerFX>().vfxPrefab;
            
            MaterialUtils.ApplySNShaders(model);
            
            prefab.Set(model);
        }
    }
}