using Data;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public int ID = -1;
    public int level = 1;

    public bool needStaffHelp = false;


    [SerializeField] BuildingSO buildingSO;
    public BuildingSO BuildingSO => buildingSO;
    List<SeatInBuilding> seats = new List<SeatInBuilding>();
    List<WaitLineInBuilding> waitLineInBuildings = new List<WaitLineInBuilding>();
    public bool IsBuilded = false;
    public float moneyEarnedPerPassenger;

    [SerializeField] UpdateIcon updateIcon;
    public UpdateIcon UpdateIcon => updateIcon;
    GameObject currentBuildingObject;
    [SerializeField] List<GameObject> destroyAfterBuildObjects = new List<GameObject>();

    public void OnStart()
    {
        SetUp();
    }

    void SetUp()
    {
        updateIcon.gameObject.SetActive(false);
        moneyEarnedPerPassenger = GetMoneyEarnedPerPassgenger();
        SetUpUpdateIcon();

        int level = GameManager.Instance.CheckBuildedBuiling(ID);
        if(level != -1)
        {
            this.level = level;
            IsBuilded = true;
            SpawnBuildingPref();
        }

        NeedStaffHelp();
        CheckUpdate();
    }

    public void SetUpUpdateIcon()
    {
        updateIcon.gameObject.SetActive(true);
        updateIcon.SetUp(this);
        updateIcon.gameObject.SetActive(false);
    }

    void SpawnBuildingPref()
    {
        currentBuildingObject = Instantiate(buildingSO.buildingPref, transform);
        IsBuilded = true;
        foreach (GameObject go in destroyAfterBuildObjects)
        {
            go.SetActive(false);
        }

        SeatInBuilding[] seats = currentBuildingObject.GetComponentsInChildren<SeatInBuilding>();
        
        this.seats.Clear();
        this.seats.AddRange(seats);

        foreach (SeatInBuilding seat in seats)
        {
            seat.SetUp(this);
        }

        WaitLineInBuilding[] waitLineInBuildings = currentBuildingObject.GetComponentsInChildren<WaitLineInBuilding>();

        this.waitLineInBuildings.Clear();
        this.waitLineInBuildings.AddRange(waitLineInBuildings);
        foreach (WaitLineInBuilding w in waitLineInBuildings)
        {
            w.OnStart();
        }


        GameManager.Instance.BakeNavMesh();
    }
    public void Build()
    {
        SpawnBuildingPref();

        GameManager.Instance.AddBuildedBuilding(buildingSO.buildCost, ID);
    }

    public void UpdateBuilding()
    {

        GameManager.Instance.UpdateBuilding(GetNextUpdateCost(), ID);
        level++;
    }

    public bool GetAvailableSeatForPassenger(PassengerAgent passengerAgent)
    {
        foreach (WaitLineInBuilding wait in waitLineInBuildings)
        {
            if (wait.HadFreeSeat())
            {
                wait.AddPassenger(passengerAgent);
                return true;
            }
        }
        foreach (WaitLineInBuilding wait in waitLineInBuildings)
        {
            if (wait.CanAddMorePassenger())
            {
                wait.AddPassenger(passengerAgent);
                return true;
            }
        }
        return false;
    }

    public SeatInBuilding GetAvailableSeatForStaff()
    {
        foreach (SeatInBuilding seat in this.seats)
        {
            if (seat.isOpen && !seat.isSeatedIn && seat.SeatType == SeatType.Staff)
            {
                return seat;
            }
        }
        return null;
    }

 
    #region GetStat
    public float GetMoneyEarnedPerPassgenger()
    {
        float money = buildingSO.baseMoneyEarned;

        //for (int i = 0; i < level; i++)
        //{
        //    money *= (1 + buildingSO.baseMoneyEarnedIncreasePercentPerLevel / 100);
        //}

        return money;
    }
    public float GetNextMoneyEarnedPerPassgenger()
    {
        float money = moneyEarnedPerPassenger;

        money *= (1 + buildingSO.baseMoneyEarnedIncreasePercentPerLevel / 100);

        return money;
    }
    public float GetNextUpdateCost()
    {
        float money = buildingSO.baseUpdateCost;

        for (int i = 0; i < level; i++)
        {
            money *= (1 + buildingSO.baseMoneyUpdateIncreasePercentPerLevel / 100);
        }

        return money;
    }
    public float GetBuildCost()
    {
        return buildingSO.buildCost;
    }
    public void CheckUpdate()
    {
        updateIcon.gameObject.SetActive(false);
        UserData userData = GameManager.Instance.UserData;
        if (userData.level >= buildingSO.unlockedLevel)
        {
            if(!IsBuilded)
            {
                if(userData.money >= buildingSO.buildCost)
                {
                    SetUpUpdateIcon();
                    updateIcon.gameObject.SetActive(true);
                }
            }
            else
            {
                if (userData.money >= GetNextUpdateCost())
                {
                    SetUpUpdateIcon();
                    updateIcon.gameObject.SetActive(true);
                }
            }
            
        }
    }

    public void CheckRemainSeatPassengerToFreeStaff()
    {
        foreach (SeatInBuilding seat in seats)
        {
            if(seat.isSeatedIn && seat.SeatType == SeatType.Passenger) return;
        }

        foreach(SeatInBuilding seat in seats)
        {
            if (seat.isSeatedIn && seat.SeatType == SeatType.Staff)
            {
                seat.Agent.OnFinishTask();

            }
        }
    }
    public void NeedStaffHelp()
    {
        if (buildingSO.needStaffHelp)
        {
           needStaffHelp = true;
            return;
        }
        needStaffHelp = false;
    }

    public bool CheckCanServeMorePassenger()
    {
        foreach (WaitLineInBuilding wait in waitLineInBuildings)
        {
            if (wait.CanAddMorePassenger())
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
