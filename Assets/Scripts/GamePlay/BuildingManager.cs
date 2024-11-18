using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] List<BuildingObject> buildingObjects = new List<BuildingObject>();

    [SerializeField] List<BuildingObject> tutorialBuildingObjects = new List<BuildingObject>();
    
    public bool NeedTutorial = false;

    private void Start()
    {
        if (!NeedTutorial)
        {
            foreach (BuildingObject buildingObject in buildingObjects)
            {
                buildingObject.OnStart();
            }
        }
        else
        {
            StartCoroutine(PlayTutorial());
        }
    }

    IEnumerator PlayTutorial()
    {
        foreach(BuildingObject buildingObject in tutorialBuildingObjects)
        {
            buildingObject.OnStart();
            buildingObject.UpdateIcon.PlayPulseEffect(Vector3.one, new Vector3(1.5f , 1.5f , 1.5f), .5f, Mathf.Infinity);
            yield return new WaitUntil(() => buildingObject.IsBuilded);
        }
    }
    
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

    public BuildingObject GetNeedStaffHelpBuilding()
    {
        foreach(BuildingObject buildingObject in buildingObjects)
        {
            if(buildingObject.needStaffHelp && buildingObject.GetAvailableSeatForStaff() != null)
            {
                return buildingObject;
            }
        }

        return null;
    }

    public BuildingObject GetNeedStaffHelpTypeBuilding(BuildingType buildingType)
    {
        foreach (BuildingObject buildingObject in buildingObjects)
        {
            if (buildingObject.needStaffHelp && buildingObject.GetAvailableSeatForStaff() != null && buildingObject.BuildingSO.buildingType == buildingType)
            {
                return buildingObject;
            }
        }

        return null;
    }

    public BuildingObject GetRandomCanServeBuilding()
    {
        List<BuildingObject> availableBuilding = new List<BuildingObject>();
        foreach (BuildingObject buildingObject in buildingObjects)
        {
            if (buildingObject.GetAvailableSeatForPassenger() != null)
            {
                availableBuilding.Add(buildingObject);
            }
        }

        if (availableBuilding.Count <= 0) return null; 
        return availableBuilding[Random.Range(0, availableBuilding.Count)];
    }
}
