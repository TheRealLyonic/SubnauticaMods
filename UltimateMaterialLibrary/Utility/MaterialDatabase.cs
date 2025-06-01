using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UWE;

namespace LyonicDevelopment.UltimateMaterialLibrary.Utility
{
    public class MaterialDatabase : MaterialDatabaseBase
    {
        //Returns the current size of the material database, if it exists. Other-wise returns -1.
        public static int currentSize
        {
            get
            {
                if (matDatabase == null)
                    return -1;
                
                return matDatabase.Count;
            }
        }
        
        public static void Initialize()
        {
            BuildDatabase();
        }

        public static IEnumerator ReplaceVanillaMats(GameObject customObject)
        {
            if (customObject == null)
            {
                Plugin.Logger.LogError("A null object was passed into the ReplaceVanillaMats method.");
                yield break;
            }

            var renderers = customObject.GetAllComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
            {
                Plugin.Logger.LogError($"No renderers were found on the custom object: ${customObject.name}");
                yield break;
            }

            List<Material> replaceMaterials = new List<Material>();
            List<string> skipMatNames = new List<string>();
            foreach (var renderer in renderers)
            {
                var newMatList = renderer.materials;

                for (int i = 0; i < newMatList.Length; i++)
                {
                    if (newMatList[i] == null)
                        continue;
                    
                    var currentMatName = RemoveInstanceFromMatName(newMatList[i].name);

                    bool skipMat = skipMatNames.Contains(currentMatName);
                    if (!skipMat)
                    {
                        foreach (var mat in replaceMaterials)
                        {
                            if (mat.name.Equals(currentMatName))
                            {
                                newMatList[i] = mat;
                                skipMat = true;
                                break;
                            }
                        }
                    }

                    if (skipMat)
                        continue;

                    var taskResult = new TaskResult<Material>();

                    yield return TryGetMatFromDatabase(currentMatName, taskResult);

                    var foundMaterial = taskResult.value;

                    if (foundMaterial == null)
                        continue;

                    newMatList[i] = foundMaterial;
                    replaceMaterials.Add(foundMaterial);
                }

                renderer.materials = newMatList;
            }
        }

        public static IEnumerator TryGetMatFromDatabase(string matName, IOut<Material> materialResult)
        {
            Material matResult = null;
            
            if (matDatabase.TryGetValue(matName, out var matPath))
            {
                Material returnedMat;

                /*
                For whatever reason, even after the database is finished, and the entry for the material is found to
                exist, loading it through these means will occasionally result in a null material return value. We know
                that none of the materials are actually null, however. For this reason, we just re-attempt to load the material
                at the specified file path until it's fetched successfully. All testing indicates that this rarely happens,
                and when it does, only requires 1-4 retries max.
                */
                bool prefabMat = matPath.EndsWith(".prefab");
                do
                {
                    Plugin.Logger.LogDebug($"Trying to get mat {matName} from database...");
                    var taskResult = new TaskResult<Material>();
                    if (prefabMat)
                        yield return GetMaterialFromPrefabPath(RemoveInstanceFromMatName(matName), matPath,
                            taskResult);
                    else
                        yield return GetMaterialFromPath(matPath, taskResult);

                    returnedMat = taskResult.value;
                } while (returnedMat == null);

                matResult = returnedMat;
            }
            else
                Plugin.Logger.LogDebug($"Failed to find material {matName} in material database.");
            
            materialResult.Set(matResult);
        }

        public static IEnumerator TryGetMatFromDatabase(int matIndex, IOut<Material> materialResult)
        {
            Material matResult = null;

            if (matIndex >= 0 && matIndex < matDatabase.Count)
            {
                var taskResult = new TaskResult<Material>();
                
                yield return TryGetMatFromDatabase(matDatabase.ElementAt(matIndex).Key, taskResult);
                
                var foundMat = taskResult.value;
                
                if(foundMat == null)
                    Plugin.Logger.LogError($"Failed to get material at index {matIndex} from matDatabase.");
                
                matResult = foundMat;
            }else
                Plugin.Logger.LogError($"ERROR: Material index {matIndex} is outside the bounds of the material database.");
            
            materialResult.Set(matResult);
        }
    }
    
    public abstract class MaterialDatabaseBase
    {
        public static readonly int VANILLA_MAT_DATABASE_SIZE = 2838;
        
        public static int FINAL_DATABASE_SIZE;
        
        protected static Dictionary<string, string> matDatabase = new Dictionary<string, string>();

        protected static void BuildDatabase()
        {
            //Registers vanilla mats - 2,838 of them.
            var filePathMap = new FileInfo(Plugin.MOD_FOLDER_PATH + "/Resources/MatFilePathMap.json");

            if (!filePathMap.Exists)
            {
                Plugin.Logger.LogError($"Failed to get the FilePathMap at path {filePathMap.FullName}!");
                return;
            }

            var streamReader = new StreamReader(filePathMap.FullName);

            foreach (var entry in JObject.Parse(streamReader.ReadToEnd()))
                RegisterMat(entry.Key, entry.Value.ToString());
        }

        protected static void RegisterMat(string matName, string filePath)
        {
            if (!matDatabase.ContainsKey(matName))
            {
                Plugin.Logger.LogDebug($"Registered material #{matDatabase.Count}: {matName}.");
                matDatabase.Add(matName, filePath);
            }
        }

        protected static IEnumerator GetMaterialFromPath(string matPath, IOut<Material> materialResult)
        {
            materialResult.Set(null);
            
            if (!matPath.EndsWith(".mat"))
            {
                Plugin.Logger.LogError($"ERROR: {matPath} is not a path to a valid material.");
                yield break;
            }
            
            var handle = AddressablesUtility.LoadAsync<Material>(matPath);

            yield return handle.Task;
            
            materialResult.Set(handle.Result);
        }

        protected static IEnumerator GetMaterialFromPrefabPath(string matName, string prefabPath, IOut<Material> materialResult)
        {
            var startTime = DateTime.UtcNow;
            
            materialResult.Set(null);
            
            if (!prefabPath.EndsWith(".prefab"))
            {
                Plugin.Logger.LogError($"ERROR: {prefabPath} is not a path to a valid prefab.");
                yield break;
            }

            var task = PrefabDatabase.GetPrefabForFilenameAsync(prefabPath);

            yield return task;

            if (task.TryGetPrefab(out var prefab))
            {
                var renderers = prefab.GetAllComponentsInChildren<Renderer>();

                foreach (var renderer in renderers)
                {
                    foreach (var mat in renderer.materials)
                    {
                        if (mat == null)
                            continue;

                        if (RemoveInstanceFromMatName(mat.name).Equals(matName))
                        {
                            materialResult.Set(mat);
                            
                            var timeSpan = DateTime.UtcNow - startTime;
                            
                            Plugin.Logger.LogWarning($"Getting material {matName} from prefab took {timeSpan.TotalMilliseconds}ms");
                            
                            yield break;
                        }
                    }
                }
                
                Plugin.Logger.LogError($"Failed to find material {matName} on prefab at path: {prefabPath}!");
                yield break;
            }
            
            Plugin.Logger.LogError($"Failed to get prefab at path {prefabPath} from PrefabDatabase.");
        }
        
        protected static string RemoveInstanceFromMatName(string originalMatName)
        {
            string returnValue = originalMatName;
            
            if(originalMatName.EndsWith(" (Instance)"))
                returnValue = originalMatName.Substring(0, originalMatName.Length - " (Instance)".Length);

            return returnValue;
        }
    }
}