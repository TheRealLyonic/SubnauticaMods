namespace LyonicDevelopment.UltimateMaterialLibrary.Mono
{
    public class WBOITPreview : WBOIT
    {
        private new void Awake()
        {
            if (compositeShader == null || temperatureRefractTex == null)
            {
                var playerWBOIT = Player.main.camRoot.mainCam.GetComponent<WBOIT>();

                compositeShader = playerWBOIT.compositeShader;
                temperatureRefractTex = playerWBOIT.temperatureRefractTex;
            }
            
            base.Awake();
        }
    }
}