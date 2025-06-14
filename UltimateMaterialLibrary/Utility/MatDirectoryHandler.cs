using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            return returnList;
        }
        
        public static IEnumerator GetAllMaterialsInsideDirectory(string directoryPath, TaskResult<List<Material>> materialList)
        {
            var returnList = new List<Material>();

            if (!directoryMaterials.ContainsKey(directoryPath))
                yield break;

            var materialNames = directoryMaterials[directoryPath];

            if (directoryPath.Contains("Prefabs/"))
            {
                var task = new TaskResult<List<Material>>();
                yield return MaterialDatabase.GetAllMaterialsFromPrefab(MaterialDatabase.GetMaterialPath(materialNames[0]), task);

                var foundMaterials = task.value;

                if (foundMaterials == null)
                {
                    Plugin.Logger.LogError($"Failed to get list of materials inside directory {directoryPath}.");
                    yield break;
                }
                
                returnList.AddRange(foundMaterials);
            }
            else
            {
                foreach (var materialName in materialNames)
                {
                    var taskResult = new TaskResult<Material>();
                    yield return MaterialDatabase.TryGetMatFromDatabase(materialName, taskResult);

                    var foundMat = taskResult.value;

                    if (foundMat == null)
                    {
                        Plugin.Logger.LogError($"Failed to get material {materialName} from database.");
                        yield break;
                    }
                    
                    returnList.Add(foundMat);
                }
            }
            
            materialList.Set(returnList);
        }
    }
}