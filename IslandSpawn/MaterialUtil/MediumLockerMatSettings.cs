using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.MaterialUtil
{
    public class MediumLockerMatSettings : MaterialModifier
    {

        public override void EditMaterial(Material material, Renderer renderer, int matIndex,
            MaterialUtils.MaterialType matType)
        {
            if (material.name.Contains("Locker_Main"))
            {
                material.SetFloat(MaterialProperties.FRESNEL, 1f);
                
                material.SetFloat(MaterialProperties.GLOW_STRENGTH_NIGHT, 5f);
            }
        }
        
    }
}