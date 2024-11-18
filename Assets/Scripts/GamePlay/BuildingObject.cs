using Data;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public int ID = -1;
    public int level = 1;
    public bool needStaffHelp = false;
    public bool hadStaffHelp = false;

    [SerializeField] BuildingSO buildingSO;
    public BuildingSO BuildingSO => buildingSO;
    List<SeatInBuilding> seats = new List<SeatInBuilding>();
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

        CheckNeedStaffHelp();
        CheckUpdate();
    }

    public void SetUpUpdateIcon()
    {
        updateIcon.SetUp(this);
    }

    void SpawnBuildingPref()
    {
        currentBuildingObject = Instantiate(buildingSO.buildingPref, transform);
        IsBuilded = true;
        foreach (GameObject go in destroyAfterBuildObjects)
        {
            Destroy(go);
        }

        SeatInBuilding[] seats = currentBuildingObject.GetComponentsInChildren<SeatInBuilding>();
        this.seats.Clear();
        this.seats.AddRange(seats);

        foreach (SeatInBuilding seat in seats)
        {
            seat.SetUp(this);
        }
    }
    public void Build()
    {
        SpawnBuildingPref();

        GameManager.Instance.AddBuildedBuilding(buildingSO.buildCost, ID);
    }

    public SeatInBuilding GetAvailableSeatForPassenger()
    {
        foreach(SeatInBuilding seat in this.seats)
        {
            if(seat.isOpen && !seat.isSeatedIn && seat.SeatType == SeatType.Passenger)
            {
                return seat;
            }
        }
        return null;
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

        for (int i = 0; i < level; i++)
        {
            money *= (1 + buildingSO.baseMoneyEarnedIncreasePercentPerLevel / 100);
        }

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
                    updateIcon.gameObject.SetActive(true);
                }
            }
            else
            {
                if (userData.money >= GetNextUpdateCost())
                {
                    updateIcon.gameObject.SetActive(true);
                }
            }
            
        }
    }
    public void SetHadStaffHelp(bool b)
    {
        hadStaffHelp = b;
    }
    public void CheckRemainSeatPassengerToFreeStaff()
    {
        foreach (SeatInBuilding seat in seats)
        {
            if(seat.isOpen && seat.SeatType == SeatType.Passenger) return;
        }

        foreach(SeatInBuilding seat in seats)
        {
            if (seat.isOpen && seat.SeatType == SeatType.Staff)
            {
                StaffAgent staffAgent = (StaffAgent)seat.Agent;
                staffAgent.ChangeState(StaffState.Free);
            }
        }
    }
    public void CheckNeedStaffHelp()
    {
        if (buildingSO.needStaffHelp) needStaffHelp = true;
    }
    #endregion
}
