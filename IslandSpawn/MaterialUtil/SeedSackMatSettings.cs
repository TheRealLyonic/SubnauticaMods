using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.MaterialUtil
{
    public class SeedSackMatSettings : MaterialModifier
    {

        public override void EditMaterial(Material material, Renderer renderer, int matIndex,
            MaterialUtils.MaterialType matType)
        {
            if (material.name.Contains("Kelp"))
            {
                material.SetColor(MaterialProperties.GLOW_COLOR, new Color(0.666f, 0.952f, 0f));
                material.SetColor(MaterialProperties.SPEC_COLOR, new Color(0.558f, 1f, 0.689f));

                material.SetFloat(MaterialProperties.SPEC_ENUM, 3f);
                material.SetFloat(MaterialProperties.SHININESS, 3.73f);
                material.SetFloat(MaterialProperties.FRESNEL, 0f);
                material.SetFloat(MaterialProperties.GLOW_STRENGTH, 1f);
                material.SetFloat(MaterialProperties.GLOW_STRENGTH_NIGHT, 0.2f);
            }else if (material.name.Contains("SeedSack"))
            {
                material.SetColor(MaterialProperties.SPEC_COLOR, new Color(0.556f, 1f, 0.689f));
                
                material.SetFloat(MaterialProperties.SHININESS, 7.1718f);
                material.SetFloat(MaterialProperties.FRESNEL, 0.3571f);
                material.SetFloat(MaterialProperties.GLOW_STRENGTH, 1f);
                material.SetFloat(MaterialProperties.GLOW_STRENGTH_NIGHT, 1f);
                material.SetFloat(MaterialProperties.LIGHTMAP_STRENGTH, 2.65f);
            }
            else 
                Plugin.Logger.LogError($"Material name error on {renderer.gameObject}.");
        }
        
    }
}