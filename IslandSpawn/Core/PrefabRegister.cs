using System.Collections;
using LyonicDevelopment.IslandSpawn.Prefabs;
using Nautilus.Assets;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.IslandSpawn.Core
{
    public static class PrefabRegister
    {
        public static GameObject DegasiHabitatSky { get; private set; }
        
        public static RustedSolarPanel rustedSolarPanel { get; private set; } = new RustedSolarPanel(PrefabInfo.WithTechType("RustedSolarPanel"));
        public static RustedFabricator rustedFabricator { get; private set; } = new RustedFabricator(PrefabInfo.WithTechType("RustedFabricator"));
        public static RustedRadio rustedRadio { get; private set; } = new RustedRadio(PrefabInfo.WithTechType("RustedRadio"));
        public static RustedMedCabinet rustedMedCabinet { get; private set; } = new RustedMedCabinet(PrefabInfo.WithTechType("RustedMedCabinet"));
        public static CaveVine caveVine { get; private set; } = new CaveVine(PrefabInfo.WithTechType("CaveVine"));
        public static RustedStorageLocker rustedStorageLocker { get; private set; } = new RustedStorageLocker(PrefabInfo.WithTechType("RustedStorageLocker"));
        
        public static void RegisterPrefabs()
        {
            CoroutineHost.StartCoroutine(RegisterPrefabs_Internal());
        }

        private static IEnumerator RegisterPrefabs_Internal()
        {
            var task = PrefabDatabase.GetPrefabAsync("569f22e0-274d-49b0-ae5e-21ef0ce907ca");
            yield return task;

            task.TryGetPrefab(out var degasiHabitat);
            
            if(!degasiHabitat)
                Plugin.Logger.LogError("Failed to load the prefab for the DegasiHabitat.");

            foreach (var skyApplier in degasiHabitat.GetComponents<SkyApplier>())
            {
                if (skyApplier.customSkyPrefab != null)
                {
                    DegasiHabitatSky = skyApplier.customSkyPrefab;
                    break;
                }
            }
            
            // caveVine.Register();
            // rustedSolarPanel.Register();
            // rustedFabricator.Register();
            rustedRadio.Register();
            rustedMedCabinet.Register();
            // rustedStorageLocker.Register();
        }

    }
}