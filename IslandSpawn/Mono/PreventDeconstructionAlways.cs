namespace LyonicDevelopment.IslandSpawn.Mono
{
    public class PreventDeconstructionAlways : PreventDeconstruction
    {
        
        public bool always;

        private new void Start()
        {
            Constructable constructable = gameObject.GetComponent<Constructable>();
            
            if (constructable != null && always) 
                constructable.deconstructionAllowed = false;
            else 
                base.Start();
        }

    }
}