using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RemoveInactiveObjectsEditor : EditorWindow
{
    [MenuItem("Tools/Remove Inactive Objects")]
    public static void ShowWindow()
    {
        GetWindow<RemoveInactiveObjectsEditor>("Remove Inactive Objects");
    }

    private void OnGUI()
    {
        GUILayout.Label("Remove Inactive GameObjects", EditorStyles.boldLabel);

        if (GUILayout.Button("Remove All Inactive Objects"))
        {
            RemoveAllInactiveObjects();
        }
    }

    private void RemoveAllInactiveObjects()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); 
        int removedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            if (!obj.activeInHierarchy)
            {
                DestroyImmediate(obj);
                removedCount++;
            }
        }

        Debug.Log($"Đã xóa {removedCount} đối tượng không hoạt động.");
    }
}
