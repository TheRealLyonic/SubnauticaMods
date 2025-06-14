using System.Collections;
using Nautilus.Utility;
using UnityEngine;
using Random = System.Random;

namespace LyonicDevelopment.UltimateMaterialLibrary.Mono.UI.PreviewHandler
{
    public class PreviewObjectHandler : MonoBehaviour
    {
        public MatPreviewImageGenerator PreviewImageGenerator { get; private set; }
        
        private const float FORWARD_DISTANCE = 3f;
        
        [SerializeField]
        private GameObject previewParentPrefab;

        private GameObject previewParent;
        private GameObject currentPreviewObj;

        private SkyApplier previewObjectSky;

        private GameObject hoveredObject;
        private GameObject previousHoveredObject;
        private Material previousMat;
        
        public void SpawnPreview(Transform camControllerTr)
        {
            previewParent = Instantiate(previewParentPrefab, camControllerTr);

            PreviewImageGenerator = previewParent.GetComponent<MatPreviewImageGenerator>();

            previewParent.transform.position = camControllerTr.position + camControllerTr.forward * FORWARD_DISTANCE;

            previewObjectSky = previewParent.GetComponent<SkyApplier>();

            StartCoroutine(SpawnPrimitiveShape(PrimitiveType.Sphere));
        }

        public void ParentToCam(Transform camControllerTr)
        {
            previewParent.transform.SetParent(camControllerTr);
        }

        public void UnparentFromCam()
        {
            previewParent.transform.SetParent(null);
        }

        public void UpdatePreviewObjectLocation(Transform camControllerTr)
        {
            previewParent.transform.position = camControllerTr.position + camControllerTr.forward * FORWARD_DISTANCE;
            
            previewObjectSky.UpdateSkyIfNecessary();
        }

        public void UpdateHoveredObject(GameObject hoveredObject)
        {
            this.hoveredObject = hoveredObject;
        }

        public void ResetHoveredObjectMaterial()
        {
            if (previousHoveredObject != null)
            {
                var renderer = previousHoveredObject.GetComponent<Renderer>();

                if (renderer is null)
                    return;

                renderer.material = previousMat;
                
                previousMat = null;
                previousHoveredObject = null;
            }
        }

        public void UpdateHoveredObjectMaterial(Material newMaterial)
        {
            if (hoveredObject == null || previousHoveredObject != hoveredObject)
                ResetHoveredObjectMaterial();
            
            if (hoveredObject != null)
            {
                var renderer = hoveredObject.GetComponent<Renderer>();

                if (renderer is null)
                    return;

                if (Utility.MaterialDatabase.FilterInstanceFromMatName(renderer.material.name) != Utility.MaterialDatabase.FilterInstanceFromMatName(newMaterial.name))
                {
                    previousHoveredObject = hoveredObject;
                    previousMat = renderer.material;
                    renderer.material = newMaterial;
                }
            }
        }

        public void LockHoveredObjectMaterial()
        {
            previousMat = null;
            previousHoveredObject = null;
        }

        //TODO: Make pop-up informing the user to enable read/write property on .fbx if Mesh is not read/writable.
            //"Mesh has been marked as non-accessible."
        private IEnumerator SpawnPrimitiveShape(PrimitiveType primitiveType)
        {
            currentPreviewObj = Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("Reaper_Model.prefab"));
            
            MaterialUtils.ApplySNShaders(currentPreviewObj);
            // currentPreviewObj = GameObject.CreatePrimitive(primitiveType);

            currentPreviewObj.name = "PreviewObject";
            currentPreviewObj.transform.SetParent(previewParent.transform, false);

            //Disable any existing colliders
            foreach (var collider in currentPreviewObj.GetComponentsInChildren<Collider>(true))
                collider.enabled = false;
            
            var renderers = currentPreviewObj.GetComponentsInChildren<Renderer>();
            
            previewObjectSky.renderers = renderers;

            foreach (var meshFilter in currentPreviewObj.GetComponentsInChildren<MeshFilter>())
            {
                var collider = meshFilter.gameObject.AddComponent<MeshCollider>();
                
                collider.sharedMesh = meshFilter.sharedMesh;

                var hoverDetector = meshFilter.gameObject.AddComponent<MouseHoverDetector>();
                
                hoverDetector.SetHandler(this);
            }

            var matTask = new TaskResult<Material>();
            yield return Utility.MaterialDatabase.TryGetMatFromDatabase(
                new Random().Next(Utility.MaterialDatabase.currentSize - 1), matTask);
            
            renderers[0].material = matTask.value;
            
            currentPreviewObj.SetActive(true);
        }

    }
}