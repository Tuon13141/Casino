using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] List<BuildingObject> buildingObjects = new List<BuildingObject>(); 

    public void AddBuildingObject(BuildingObject buildingObject)
    {
        buildingObjects.Add(buildingObject);
    }

    public void CheckBuildingUpdate()
    {
        foreach(BuildingObject buildingObject in buildingObjects)
        {
            buildingObject.CheckUpdate();
        }
    }
}
