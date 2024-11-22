using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] List<BuildingObject> buildingObjects = new List<BuildingObject>();

    [SerializeField] List<BuildingObject> tutorialBuildingObjects = new List<BuildingObject>();
    
    public bool NeedTutorial = false;

    [SerializeField] List<BuildingObject> receptionistAreas = new List<BuildingObject>();
    private void Start()
    {
        NeedTutorial = GameManager.Instance.UserData.needTutorial;
        if (!NeedTutorial)
        {
            foreach (BuildingObject buildingObject in buildingObjects)
            {
                buildingObject.OnStart();
            }
            foreach (BuildingObject buildingObject in receptionistAreas)
            {
                buildingObject.OnStart();
            }

            PassengerManager.Instance.canSpawnPassenger = true;
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

        PassengerManager.Instance.canSpawnPassenger = true;
        GameManager.Instance.UserData.needTutorial = false ;
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
        foreach (BuildingObject buildingObject in receptionistAreas)
        {
            buildingObject.CheckUpdate();
        }
    }

    public BuildingObject GetNeedStaffHelpBuilding()
    {
        foreach (BuildingObject buildingObject in receptionistAreas)
        {
            if (buildingObject.needStaffHelp && buildingObject.GetAvailableSeatForStaff() != null && buildingObject.IsBuilded)
            {
                return buildingObject;
            }
        }
        foreach (BuildingObject buildingObject in buildingObjects)
        {
            if(buildingObject.needStaffHelp && buildingObject.GetAvailableSeatForStaff() != null && buildingObject.IsBuilded)
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
            if (buildingObject.needStaffHelp && buildingObject.GetAvailableSeatForStaff() != null && buildingObject.BuildingSO.buildingType == buildingType && buildingObject.IsBuilded)
            {
                return buildingObject;
            }
        }

        return null;
    }

    public BuildingObject GetRandomBuildingForPassenger()
    {
        List<BuildingObject> availableBuilding = new List<BuildingObject>();
        foreach (BuildingObject buildingObject in buildingObjects)
        {
            if(buildingObject.IsBuilded)
                availableBuilding.Add(buildingObject);
        }

        if (availableBuilding.Count <= 0) return null; 
        return availableBuilding[Random.Range(0, availableBuilding.Count)];
    }

    public BuildingObject GetRandomReceptionistAreaForPassenger()
    {
        List<BuildingObject> availableBuilding = new List<BuildingObject>();
        foreach (BuildingObject buildingObject in receptionistAreas)
        {
            if (!buildingObject.CheckCanServeMorePassenger() || !buildingObject.IsBuilded) continue;
            availableBuilding.Add(buildingObject);
        }

        if (availableBuilding.Count <= 0) return null;
        return availableBuilding[Random.Range(0, availableBuilding.Count)];
    }
}
