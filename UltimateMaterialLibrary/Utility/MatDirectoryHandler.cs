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

        public static string[] GetAllMaterialsInsideDirectory(string directoryPath)
        {
            directoryMaterials.TryGetValue(directoryPath, out var matNames);

            return matNames;
        }
    }
}