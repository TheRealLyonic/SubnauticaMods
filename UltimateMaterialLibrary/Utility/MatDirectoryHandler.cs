using System;
using System.Collections.Generic;

namespace LyonicDevelopment.UltimateMaterialLibrary.Utility
{
    public class MatDirectoryHandler
    {
        private static Dictionary<string, string[]> directoryMaterials = new Dictionary<string, string[]>();

        protected static void RegisterMaterialWithDirectory(string matDirectory, string matName)
        {
            if (directoryMaterials.TryGetValue(matDirectory, out var previousMatNames))
            {
                string[] newMatNames = new string[previousMatNames.Length + 1];
                
                Array.Copy(previousMatNames, newMatNames, previousMatNames.Length);
                newMatNames[previousMatNames.Length] = matName;
                
                directoryMaterials[matDirectory] = newMatNames;
            }else
                directoryMaterials.Add(matDirectory, new []{ matName });
        }
        
        public static List<string> GetAllFoldersInsideDirectory(string directoryPath)
        {
            var returnList = new List<string>();

            foreach (var path in directoryMaterials.Keys)
            {
                if (path.StartsWith(directoryPath))
                {
                    if (directoryPath.Length == path.Length)
                        continue;
                    
                    var trimmedPath = path.Substring(directoryPath.Length);

                    //This is not a folder inside the directory we're in. Just starts with the same string.
                    if (!trimmedPath[0].Equals('/'))
                        continue;
                    
                    trimmedPath = trimmedPath.Substring(1);

                    var index = trimmedPath.IndexOf('/');
                    
                    trimmedPath = index > 0 ? trimmedPath.Substring(0, index) : trimmedPath;
                    
                    if(!returnList.Contains(trimmedPath))
                        returnList.Add(trimmedPath);
                }
            }

            if(returnList.Count > 0)
                returnList.Sort();
            
            return returnList;
        }

        public static List<string> GetAllFoldersThatContain(string currentDirectory, string searchString)
        {
            var returnList = new List<string>();

            var potentialFolders = GetFoldersRecursively(currentDirectory);

            foreach (var folder in potentialFolders)
            {
                var rawFolderName = folder.Substring(folder.LastIndexOf('/') + 1);
                
                if (rawFolderName.ToLower().Contains(searchString.ToLower()))
                    returnList.Add(folder);
            }
            
            returnList.Sort();
            
            return returnList;
        }

        public static string[] GetAllMaterialsInsideDirectory(string directoryPath)
        {
            directoryMaterials.TryGetValue(directoryPath, out var matNames);
            
            if(matNames != null)
                Array.Sort(matNames);
            
            return matNames;
        }
        
        public static List<string> GetAllMaterialsThatContain(string currentDirectory, string searchString)
        {
            List<string> matResults = new List<string>();
            
            foreach (var path in directoryMaterials.Keys)
            {
                if (path.StartsWith(currentDirectory))
                {
                    foreach (var material in directoryMaterials[path])
                    {
                        if (material.ToLower().Contains(searchString.ToLower()))
                            matResults.Add(material);
                    }
                }
            }
            
            return matResults;
        }

        private static List<string> GetFoldersRecursively(string parentDirectory)
        {
            List<string> result = new List<string>();

            foreach (var folder in GetAllFoldersInsideDirectory(parentDirectory))
            {
                foreach (var subFolder in GetFoldersRecursively(parentDirectory + "/" + folder))
                    result.Add(folder + "/" + subFolder);
                
                result.Add(folder);
            }
            
            return result;
        }
    }
}