using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] List<BuildingObject> buildingObjects = new List<BuildingObject>();

    [SerializeField] List<BuildingObject> tutorialBuildingObjects = new List<BuildingObject>();

    bool firstLoad = true;
    public bool FirstLoad { get { return firstLoad; }
                            set
                            {
                                firstLoad = value;
                                foreach (BuildingObject buildingObject in receptionistAreas)
                                {
                                    if(buildingObject.IsBuilded)
                                        buildingObject.FirstLoad = false;
                                }
                                foreach (BuildingObject buildingObject in buildingObjects)
                                {
                                    if (buildingObject.IsBuilded)
                                        buildingObject.FirstLoad = false;
                                }
                            }
    }
    public bool NeedTutorial = false;
    public bool NeedTutorialPointer { get; set; } = false;
    public bool nextTutorial = false;
    [SerializeField] GameObject pointer;
    private GameObject currentPointer;

    [SerializeField] List<BuildingObject> receptionistAreas = new List<BuildingObject>();

    [SerializeField] List<GameObject> cameraEqualScaleObjects = new List<GameObject>();
   

    private void Start()
    {
        NeedTutorial = GameManager.Instance.UserData.needTutorial;
        if (!NeedTutorial)
        {
            NeedTutorialPointer = false;
            foreach (BuildingObject buildingObject in buildingObjects)
            {
                buildingObject.OnStart();
            }
            foreach (BuildingObject buildingObject in receptionistAreas)
            {
                buildingObject.OnStart();
            }
            Game.Update.AddTask(AdjustObjectHeight);
            PassengerManager.Instance.canSpawnPassenger = true;
        }
        else
        {
            StartCoroutine(PlayTutorial());
        }

     
    }

    IEnumerator PlayTutorial()
    {
        GameUI.Instance.Get<UIInGame>().ShowUpdateButton(false);
        for (int i = 0; i < tutorialBuildingObjects.Count; i++)
        {
            BuildingObject buildingObject = tutorialBuildingObjects[i];
            buildingObject.OnStart();

            if (buildingObject.UpdateIcon.isActiveAndEnabled)
            {
                buildingObject.UpdateIcon.PlayPulseEffect(Vector3.one, new Vector3(1.5f, 1.5f, 1.5f), .5f, Mathf.Infinity);
            }

            

            yield return new WaitUntil(() => buildingObject.IsBuilded);

            if (i == 0)
            {
                tutorialBuildingObjects[0].CheckUpdate();
                GameUI.Instance.Get<UITutorial>().Show();
                GameUI.Instance.Get<UITutorial>().PlayStageOne();
                if (buildingObject.UpdateIcon.isActiveAndEnabled)
                {
                    buildingObject.UpdateIcon.PlayPulseEffect(Vector3.one, new Vector3(1.5f, 1.5f, 1.5f), .5f, Mathf.Infinity);
                }
            }
            yield return new WaitUntil(() => nextTutorial);
            if(i == 0)
            {
                GameUI.Instance.Get<UITutorial>().Hide();
            }
        }

        firstLoad = false;
        PassengerManager.Instance.canSpawnPassenger = true;
        GameManager.Instance.UserData.needTutorial = false ;
        NeedTutorial = false;
        NeedTutorialPointer = true;
        Game.Update.AddTask(AdjustObjectHeight);
        GameUI.Instance.Get<UIInGame>().ShowUpdateButton(true);

        
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
        if (firstLoad)
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
                if (buildingObject.needStaffHelp && buildingObject.GetAvailableSeatForStaff() != null && buildingObject.IsBuilded)
                {
                    return buildingObject;
                }
            }
          
        }
        else
        {
            foreach (BuildingObject buildingObject in buildingObjects)
            {
                Debug.Log(buildingObject.gameObject.name);
                if (buildingObject.needStaffHelp && buildingObject.GetAvailableSeatForStaff() != null && buildingObject.IsBuilded && buildingObject.CheckNeedStaffHelp())
                {
                    Debug.Log("Found : " + buildingObject.gameObject.name);
                    return buildingObject;
                }
            }
            foreach (BuildingObject buildingObject in receptionistAreas)
            {
                if (buildingObject.needStaffHelp && buildingObject.GetAvailableSeatForStaff() != null && buildingObject.IsBuilded && buildingObject.CheckNeedStaffHelp())
                {
                    return buildingObject;
                }
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

    public void AddToCameraEqualScaleObject(GameObject gameObject)
    {
        if(cameraEqualScaleObjects.Contains(gameObject)) return;
        cameraEqualScaleObjects.Add(gameObject);
    }

    public float targetSize = 1f;
    void AdjustObjectHeight()
    {
        Camera mainCamera = Camera.main;
        foreach (var obj in cameraEqualScaleObjects)
        {
            Vector3 objectPosition = obj.transform.position;
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(objectPosition);

            // Kiểm tra object có trong camera hay không (trong viewport từ (0, 0) đến (1, 1))
            if (viewportPoint.z > 0 && viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1)
            {
                // Tính khoảng cách từ camera đến object
                float distanceToCamera = Vector3.Distance(mainCamera.transform.position, objectPosition);

                // Tính toán scale để đảm bảo kích thước bằng targetSize
                float scaleFactor = (distanceToCamera * Mathf.Tan(mainCamera.fieldOfView * 0.2f * Mathf.Deg2Rad)) / targetSize;

                // Áp dụng scale mới
                obj.transform.localScale = Vector3.one * scaleFactor;
            }
        }
    }

    public void PlaceTutorialPointer(Transform transform)
    {
        if(!NeedTutorialPointer) return;
        currentPointer = Instantiate(pointer, transform);
        currentPointer.transform.localPosition = Vector3.zero;
        NeedTutorialPointer = false;
        PassengerManager.Instance.canSpawnPassenger = false;
        GameUI.Instance.Get<UITutorial>().Show();
        GameUI.Instance.Get<UITutorial>().PlayStageTwo();
    }

    public void DestroyTutorialPointer()
    {
        if(currentPointer == null) return;  
        Destroy(currentPointer);
        PassengerManager.Instance.ResetSpeed();
        PassengerManager.Instance.canSpawnPassenger = true;
        GameUI.Instance.Get<UITutorial>().Hide();

        foreach (BuildingObject b in buildingObjects)
        {
            if (tutorialBuildingObjects.Contains(b))
            {
                continue;
            }
            b.OnStart();
        }
    }
}
