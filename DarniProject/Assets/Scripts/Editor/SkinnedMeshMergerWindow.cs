#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SkinnedMeshMergerWindow : EditorWindow
{
    private GameObject characterPrefab;

    [MenuItem("Tools/Skinned Mesh Merger")]
    public static void ShowWindow()
    {
        GetWindow<SkinnedMeshMergerWindow>("Skinned Mesh Merger");
    }

    private void OnGUI()
    {
        GUILayout.Label("Merge all SkinnedMeshRenderers in a prefab", EditorStyles.boldLabel);

        characterPrefab = (GameObject)EditorGUILayout.ObjectField("Character Prefab", characterPrefab, typeof(GameObject), false);

        if (characterPrefab == null)
        {
            EditorGUILayout.HelpBox("Select a prefab to merge its Skinned Meshes.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Merge Skinned Meshes"))
        {
            MergeSkinnedMeshes(characterPrefab);
        }
    }

    private void MergeSkinnedMeshes(GameObject prefab)
    {
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        SkinnedMeshRenderer[] parts = instance.GetComponentsInChildren<SkinnedMeshRenderer>();

        if (parts.Length <= 1)
        {
            Debug.LogWarning("Prefab has 0 or 1 SkinnedMeshRenderer. Nothing to merge.");
            DestroyImmediate(instance);
            return;
        }

        Mesh combinedMesh = new Mesh();
        CombineInstance[] combineInstances = new CombineInstance[parts.Length];
        Material mergedMaterial = parts[0].sharedMaterial; // assumes same material for all

        for (int i = 0; i < parts.Length; i++)
        {
            Mesh baked = new Mesh();
            parts[i].BakeMesh(baked);
            combineInstances[i].mesh = baked;
            combineInstances[i].transform = Matrix4x4.identity;
        }

        combinedMesh.CombineMeshes(combineInstances, true, true);

        SkinnedMeshRenderer mergedRenderer = instance.AddComponent<SkinnedMeshRenderer>();
        mergedRenderer.sharedMesh = combinedMesh;
        mergedRenderer.bones = parts[0].bones; // assumes all share the same skeleton
        mergedRenderer.rootBone = parts[0].rootBone;
        mergedRenderer.sharedMaterial = mergedMaterial;
        mergedRenderer.updateWhenOffscreen = true;

        // Disable original parts
        foreach (var part in parts)
        {
            part.gameObject.SetActive(false);
        }

        // Optionally, save as new prefab
        string path = EditorUtility.SaveFilePanelInProject("Save Merged Prefab", prefab.name + "_Merged", "prefab", "Choose save location");
        if (!string.IsNullOrEmpty(path))
        {
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Debug.Log("Merged prefab saved at: " + path);
        }

        DestroyImmediate(instance);
        Debug.Log("Skinned Meshes merged successfully!");
    }
}
#endif
