using System.Collections;
using LyonicDevelopment.IslandSpawn.Core;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Utility;
using rail;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn.Prefabs
{
    public class RustedRadio
    {
        private CustomPrefab prefab;
        private PrefabInfo prefabInfo;

        private readonly SpawnLocation spawnLocation = new SpawnLocation(new Vector3(-804.43f, 78.24f, -1050.7f), new Vector3(0f, 106.26f, 0f));

        public RustedRadio(PrefabInfo prefabInfo)
        {
            this.prefabInfo = prefabInfo;
        }

        public void Register()
        {
            prefab = new CustomPrefab(prefabInfo);

            prefab.SetGameObject(GetPrefab);
            prefab.SetSpawns(spawnLocation);
            
            prefab.Register();
        }

        private IEnumerator GetPrefab(IOut<GameObject> prefab)
        {
            var model = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("RustedRadio.prefab"));
            
            model.SetActive(false);

            model.GetComponent<TechTag>().type = prefabInfo.TechType;
            model.GetComponent<Constructable>().techType = prefabInfo.TechType;

            model.GetComponent<SkyApplier>().customSkyPrefab = PrefabRegister.DegasiHabitatSky;
            
            MaterialUtils.ApplySNShaders(model);
            
            //LiveMixin data
            var task = PrefabDatabase.GetPrefabAsync("5c06baec-0539-4f26-817d-78443548cc52");
            yield return task;

            task.TryGetPrefab(out var vanillaRadio);
            
            if(!vanillaRadio)
                Plugin.Logger.LogError("Failed to load the vanilla radio prefab.");

            
            var liveMixin = model.GetComponent<LiveMixin>();
            
            liveMixin.data = vanillaRadio.GetComponent<LiveMixin>().data;
            
            //Event Emitter setup
            var customASR = model.GetComponent<FMODASRPlayer>();

            var customEventEmitter = model.AddComponent<FMOD_StudioEventEmitter>();
            var vanillaEventEmitter = vanillaRadio.GetComponent<FMOD_StudioEventEmitter>();

            customEventEmitter.asset = vanillaEventEmitter.asset;
            customEventEmitter.path = vanillaEventEmitter.path;
            customEventEmitter.evt = vanillaEventEmitter.evt;

            customASR.startLoopSound = customEventEmitter;
            
            //Flare Setup
            var flarePrefab = vanillaRadio.GetComponentInChildren<ParticleSystem>().gameObject;

            var flareObj = Object.Instantiate(flarePrefab, model.transform);

            flareObj.transform.localPosition = new Vector3(-0.217f, 0.002f, 0.153f);
            flareObj.transform.localEulerAngles = new Vector3(270f, 0f, 0f);

            model.GetComponent<Radio>().flare = flareObj.GetComponent<ParticleSystem>();
            
            prefab.Set(model);
        }
    }
}