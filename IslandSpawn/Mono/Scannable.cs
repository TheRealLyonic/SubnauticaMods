using Nautilus.Handlers;
using UnityEngine;

namespace LyonicDevelopment.IslandSpawn.Mono
{
    
    public class Scannable : MonoBehaviour
    {
        public Texture2D entryImage;
        public string entryTitle;
        public string[] entryDesc = new string[1];

        public float scanTime = 2f;

        public enum PDAPath
        {
            Blueprints,
            SurvivalPackage,
            AdditionalTechnical,
            HabitatInstallations,
            Equipment,
            Vehicles,
            Power,
            IndigenousLifeforms,
            Coral,
            Fauna,
            Flora,
            Land,
            Sea,
            Exploitable,
            Herbivores,
            Carnivores,
            Rays,
            Sharks,
            Leviathans,
            OtherPredators,
            HerbivoresSmall,
            HerbivoresLarge,
            ScavengersAndParasites,
            Deceased,
            GeologicalData,
            AdvancedTheories,
            DataDownloads,
            OperationsLogs,
            PublicDocuments,
            DegasiSurvivors,
            AlterraSearchRescue,
            Corrupted,
            AuroraSurvivors,
            CodesClues,
            AlienData,
            TerminalData,
            ScanData,
            Artifacts,
            TimeCapsules,
            None
        }

        public PDAPath pdaPath = PDAPath.None;

        public bool destroyAfterScan;
        
        private void Start()
        {
            if (!PDAEncyclopedia.HasEntryData($"{gameObject.name}Ency"))
            {
                if (entryTitle == null || entryDesc == null || pdaPath == PDAPath.None)
                {
                    Plugin.Logger.LogWarning($"No entry data provided for {gameObject.name}.");
                    return;
                }

                string path = PathFromEnum();

                EnumHandler.TryGetValue(gameObject.GetComponent<PrefabIdentifier>().classId, out TechType techType);

                string formattedDesc = "";

                for(int i = 0; i < entryDesc.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(entryDesc[i]))
                    {
                        formattedDesc += "\n";
                        continue;
                    }
                
                    formattedDesc += entryDesc[i];

                    if (i != entryDesc.Length - 1)
                        formattedDesc += "\n";
                }
            
                PDAHandler.AddEncyclopediaEntry($"{gameObject.name}Ency", path, entryTitle, formattedDesc, entryImage);
                PDAHandler.AddCustomScannerEntry(techType, scanTime, destroyAfterScan, $"{gameObject.name}Ency");
            }
        }

        private string PathFromEnum()
        {
            switch (pdaPath)
            {
                case PDAPath.Blueprints:
                    return "Tech";
                case PDAPath.SurvivalPackage:
                    return "Welcome";
                case PDAPath.AdditionalTechnical:
                    return "Welcome/StartGear";
                case PDAPath.HabitatInstallations:
                    return "Tech/Habitats";
                case PDAPath.Equipment:
                    return "Tech/Equipment";
                case PDAPath.Vehicles:
                    return "Tech/Vehicles";
                case PDAPath.Power:
                    return "Tech/Power";
                case PDAPath.IndigenousLifeforms:
                    return "Lifeforms";
                case PDAPath.Coral:
                    return "Lifeforms/Coral";
                case PDAPath.Fauna:
                    return "Lifeforms/Fauna";
                case PDAPath.Flora:
                    return "Lifeforms/Flora";
                case PDAPath.Land:
                    return "Lifeforms/Flora/Land";
                case PDAPath.Sea:
                    return "Lifeforms/Flora/Sea";
                case PDAPath.Exploitable:
                    return "Lifeforms/Flora/Exploitable";
                case PDAPath.Herbivores:
                    return "Lifeforms/Fauna/Herbivores";
                case PDAPath.Carnivores:
                    return "Lifeforms/Fauna/Carnivores";
                case PDAPath.Rays:
                    return "Lifeforms/Fauna/Rays";
                case PDAPath.Sharks:
                    return "Lifeforms/Fauna/Sharks";
                case PDAPath.Leviathans:
                    return "Lifeforms/Fauna/Leviathans";
                case PDAPath.OtherPredators:
                    return "Lifeforms/Fauna/Other";
                case PDAPath.HerbivoresSmall:
                    return "Lifeforms/Fauna/SmallHerbivores";
                case PDAPath.HerbivoresLarge:
                    return "Lifeforms/Fauna/LargeHerbivores";
                case PDAPath.ScavengersAndParasites:
                    return "Lifeforms/Fauna/Scavengers";
                case PDAPath.Deceased:
                    return "Lifeforms/Fauna/Deceased";
                case PDAPath.GeologicalData:
                    return "PlanetaryGeology";
                case PDAPath.AdvancedTheories:
                    return "Advanced";
                case PDAPath.DataDownloads:
                    return "DownloadedData";
                case PDAPath.OperationsLogs:
                    return "DownloadedData/BeforeCrash";
                case PDAPath.PublicDocuments:
                    return "DownloadedData/PublicDocs";
                case PDAPath.DegasiSurvivors:
                    return "DownloadedData/Degasi";
                case PDAPath.AlterraSearchRescue:
                    return "DownloadedData/Degasi/Orders";
                case PDAPath.Corrupted:
                    return "DownloadedData/Lifepods";
                case PDAPath.AuroraSurvivors:
                    return "DownloadedData/AuroraSurvivors";
                case PDAPath.CodesClues:
                    return "DownloadedData/Codes";
                case PDAPath.AlienData:
                    return "DownloadedData/Precursor";
                case PDAPath.TerminalData:
                    return "DownloadedData/Precursor/Terminal";
                case PDAPath.ScanData:
                    return "DownloadedData/Precursor/Scan";
                case PDAPath.Artifacts:
                    return "DownloadedData/Precursor/Artifacts";
                case PDAPath.TimeCapsules:
                    return "TimeCapsules";
                default:
                    return null;
            }
        }
    }
}