namespace LyonicDevelopment.IslandSpawn.Mono.Prefabs
{
    public class RustedMedicalCabinet : MedicalCabinet
    {

        public new void OnHandClick(GUIHand hand)
        {
            if (doorOpen && hasMedKit && Player.main.HasInventoryRoom(1, 1))
            {
                CraftData.AddToInventory(TechType.FirstAidKit);

                hasMedKit = false;
                timeSpawnMedKit = DayNightCycle.main.timePassedAsFloat + medKitSpawnInterval;
                
                Invoke("ToggleDoorState", 2f);
            }else if(hasMedKit)
                ToggleDoorState();
        }
        
    }
}