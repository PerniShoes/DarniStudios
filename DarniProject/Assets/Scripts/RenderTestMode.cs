using UnityEngine;
using System.Collections.Generic;

public class RenderTestMode : MonoBehaviour
{
    [Header("Settings")]
    public bool enableTestMode = false;
    public Material grayMaterial;
    public Mesh simpleCubeMesh;

    private bool isApplied = false;
    private static Dictionary<MeshFilter, Mesh> originalMeshes = new();
    private static Dictionary<Renderer, Material[]> originalMaterials = new();

    void Start()
    {
        if (enableTestMode)
            ApplyTestMode(true);
    }

    void OnValidate()
    {
        if (Application.isPlaying)
            ApplyTestMode(enableTestMode);
    }

    void ApplyTestMode(bool state)
    {
        if (isApplied == state) return;
        isApplied = state;

        var meshFilters = FindObjectsOfType<MeshFilter>(true);

        foreach (var mf in meshFilters)
        {
            if (mf.gameObject.layer == LayerMask.NameToLayer("UI")) continue;

            var renderer = mf.GetComponent<Renderer>();
            if (renderer == null) continue;

            if (state)
            {
               
                if (!originalMeshes.ContainsKey(mf))
                    originalMeshes[mf] = mf.sharedMesh;

                if (!originalMaterials.ContainsKey(renderer))
                    originalMaterials[renderer] = renderer.sharedMaterials;

                
                if (simpleCubeMesh != null)
                    mf.sharedMesh = simpleCubeMesh;

                if (grayMaterial != null)
                    renderer.sharedMaterial = grayMaterial;

                
                if (renderer.TryGetComponent(out Animator anim))
                    anim.enabled = false;
            }
            else
            {
             
                if (originalMeshes.ContainsKey(mf))
                    mf.sharedMesh = originalMeshes[mf];

                if (originalMaterials.ContainsKey(renderer))
                    renderer.sharedMaterials = originalMaterials[renderer];

                if (renderer.TryGetComponent(out Animator anim))
                    anim.enabled = true;
            }
        }
    }
}