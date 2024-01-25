using System.Collections.Generic;
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

        private Dictionary<PDAPath, string> pathDictionary = new Dictionary<PDAPath, string>
        {
            {PDAPath.Blueprints, "Tech"},
            {PDAPath.SurvivalPackage, "Welcome"},
            {PDAPath.AdditionalTechnical, "Welcome/StartGear"},
            {PDAPath.HabitatInstallations, "Tech/Habitats"},
            {PDAPath.Equipment, "Tech/Equipment"},
            {PDAPath.Vehicles, "Tech/Vehicles"},
            {PDAPath.Power, "Tech/Power"},
            {PDAPath.IndigenousLifeforms, "Lifeforms"},
            {PDAPath.Coral, "Lifeforms/Coral"},
            {PDAPath.Fauna, "Lifeforms/Fauna"},
            {PDAPath.Flora, "Lifeforms/Flora"},
            {PDAPath.Land, "Lifeforms/Flora/Land"},
            {PDAPath.Sea, "Lifeforms/Flora/Sea"},
            {PDAPath.Exploitable, "Lifeforms/Flora/Exploitable"},
            {PDAPath.Herbivores, "Lifeforms/Fauna/Herbivores"},
            {PDAPath.Carnivores, "Lifeforms/Fauna/Carnivores"},
            {PDAPath.Rays, "Lifeforms/Fauna/Rays"},
            {PDAPath.Sharks, "Lifeforms/Fauna/Sharks"},
            {PDAPath.Leviathans, "Lifeforms/Fauna/Leviathans"},
            {PDAPath.OtherPredators, "Lifeforms/Fauna/Other"},
            {PDAPath.HerbivoresSmall, "Lifeforms/Fauna/SmallHerbivores"},
            {PDAPath.HerbivoresLarge, "Lifeforms/Fauna/LargeHerbivores"},
            {PDAPath.ScavengersAndParasites, "Lifeforms/Fauna/Scavengers"},
            {PDAPath.Deceased, "Lifeforms/Fauna/Deceased"},
            {PDAPath.GeologicalData, "PlanetaryGeology"},
            {PDAPath.AdvancedTheories, "Advanced"},
            {PDAPath.DataDownloads, "DownloadedData"},
            {PDAPath.OperationsLogs, "DownloadedData/BeforeCrash"},
            {PDAPath.PublicDocuments, "DownloadedData/PublicDocs"},
            {PDAPath.DegasiSurvivors, "DownloadedData/Degasi"},
            {PDAPath.AlterraSearchRescue, "DownloadedData/Degasi/Orders"},
            {PDAPath.Corrupted, "DownloadedData/Lifepods"},
            {PDAPath.AuroraSurvivors, "DownloadedData/AuroraSurvivors"},
            {PDAPath.CodesClues, "DownloadedData/Codes"},
            {PDAPath.AlienData, "DownloadedData/Precursor"},
            {PDAPath.TerminalData, "DownloadedData/Precursor/Terminal"},
            {PDAPath.ScanData, "DownloadedData/Precursor/Scan"},
            {PDAPath.Artifacts, "DownloadedData/Precursor/Artifacts"},
            {PDAPath.TimeCapsules, "TimeCapsules"},
        };

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
            
                PDAHandler.AddEncyclopediaEntry($"{gameObject.name}Ency", pathDictionary[pdaPath], entryTitle, formattedDesc, entryImage);
                PDAHandler.AddCustomScannerEntry(techType, scanTime, destroyAfterScan, $"{gameObject.name}Ency");
            }
        }
    }
}