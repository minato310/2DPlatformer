using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class ResizePrefabs : EditorWindow
{
    private Vector3 scaleFactor = new Vector3(0.5f, 0.5f, 0.5f);

    [MenuItem("Tools/Resize All Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<ResizePrefabs>("Resize All Prefabs");
    }

    private void OnGUI()
    {
        GUILayout.Label("Resize Prefabs", EditorStyles.boldLabel);
        scaleFactor = EditorGUILayout.Vector3Field("Scale Factor", scaleFactor);

        if (GUILayout.Button("Resize All Prefabs"))
        {
            ResizeAllPrefabs(scaleFactor);
        }
    }

    private static void ResizeAllPrefabs(Vector3 scale)
    {
        string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");

        foreach (string prefabPath in prefabPaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefabPath);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                prefab.transform.localScale = Vector3.Scale(prefab.transform.localScale, scale);
                PrefabUtility.SavePrefabAsset(prefab);
            }
        }

        Debug.Log("All prefabs resized successfully.");
    }
}
#endif
