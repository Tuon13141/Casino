using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnObjectTool : EditorWindow
{
    private FloorSpawner spawnScript; // Tham chiếu tới script chính

    [MenuItem("Tools/Spawn Object Tool")]
    public static void ShowWindow()
    {
        GetWindow<SpawnObjectTool>("Spawn Object Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn Object Tool", EditorStyles.boldLabel);

        // Chọn GameObject chứa script
        spawnScript = (FloorSpawner)EditorGUILayout.ObjectField("Target Object", spawnScript, typeof(FloorSpawner), true);

        // Nút để spawn object
        if (spawnScript != null && GUILayout.Button("Spawn Objects"))
        {
            spawnScript.SpawnObjects();
        }

        // Cảnh báo nếu chưa gán script
        if (spawnScript == null)
        {
            EditorGUILayout.HelpBox("Please assign a Target Object with the SpawnObjectEditor script.", MessageType.Warning);
        }
    }
}
