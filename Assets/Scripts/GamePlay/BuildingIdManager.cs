using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingIdManager : Singleton<BuildingIdManager>
{
    private static int currentID = 0; 

    [MenuItem("Tools/Assign BuildingObject IDs")]
    public static void AssignIDs()
    {
        BuildingObject[] buildingObjects = FindObjectsOfType<BuildingObject>();

        int maxID = 0;
        foreach (var obj in buildingObjects)
        {
            if (obj.ID != -1)
            {
                if(maxID < obj.ID) maxID = obj.ID;
            }
        }

        currentID = maxID;
        foreach (var obj in buildingObjects)
        {
           
            if (obj.ID == -1)
            {
                obj.ID = GenerateUniqueID();
                Debug.Log($"Assigned ID {obj.ID} to {obj.gameObject.name}");
                EditorUtility.SetDirty(obj); 
            }
        }

        Debug.Log($"Assigned IDs to {buildingObjects.Length} objects.");
    }

    private static int GenerateUniqueID()
    {
        return currentID++;
    }
}
