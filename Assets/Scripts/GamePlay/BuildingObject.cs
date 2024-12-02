﻿using Data;
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

    [SerializeField] GameObject lockObject;

    public bool FirstLoad { get; set; } = true;
    public void OnStart()
    {
        SetUp();
    }

    void SetUp()
    {
        updateIcon.gameObject.SetActive(false);
        GetMoneyEarnedPerPassgenger();
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
        BuildingManager.Instance.AddToCameraEqualScaleObject(updateIcon.gameObject);
        updateIcon.SetUp(this);
        updateIcon.gameObject.SetActive(false);
    }

    void SpawnBuildingPref()
    {
        currentBuildingObject = Instantiate(buildingSO.buildingPref, transform);
        IsBuilded = true;
        foreach (GameObject go in destroyAfterBuildObjects)
        {
            Destroy(go);
        }

        destroyAfterBuildObjects.Clear();
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
        updateIcon.gameObject.SetActive(false);
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
        GetMoneyEarnedPerPassgenger();
        GameUI.Instance.Get<UIUpdateBuilding>().SetUp(GetNextUpdateCost(), GetNextMoneyEarnedPerPassgenger(), UpdateBuilding, level, transform);
    }

    public bool GetAvailableSeatForPassenger(PassengerAgent passengerAgent)
    {
        foreach (WaitLineInBuilding wait in waitLineInBuildings)
        {
            if (wait.HadFreeSeat() && wait.HadStaff())
            {
                wait.AddPassenger(passengerAgent);
                return true;
            }
        }

        List<WaitLineInBuilding> tmp = new List<WaitLineInBuilding>();
        foreach (WaitLineInBuilding wait in waitLineInBuildings)
        {
            if (wait.CanAddMorePassenger() && wait.HadStaff())
            {
                tmp.Add(wait);
                //wait.AddPassenger(passengerAgent);
                //return true;
            }
        }

        if(tmp.Count > 0)
        {
            tmp[Random.Range(0, tmp.Count)].AddPassenger(passengerAgent);
            return true;
        }
        foreach (WaitLineInBuilding wait in waitLineInBuildings)
        {
            if (wait.HadFreeSeat())
            {
                tmp.Add(wait);
            }
        }
        if (tmp.Count > 0)
        {
            tmp[Random.Range(0, tmp.Count)].AddPassenger(passengerAgent);
            return true;
        }
        foreach (WaitLineInBuilding wait in waitLineInBuildings)
        {
            if (wait.CanAddMorePassenger())
            {
                tmp.Add(wait);
                //wait.AddPassenger(passengerAgent);
                //return true;
            }
        }
        if (tmp.Count > 0)
        {
            tmp[Random.Range(0, tmp.Count)].AddPassenger(passengerAgent);
            return true;
        }
        return false;
    }

    public SeatInBuilding GetAvailableSeatForStaff()
    {
        for (int i = 0; i < seats.Count; i++)
        {
            SeatInBuilding seat = seats[i];
            if (seat.isOpen && !seat.isSeatedIn && seat.SeatType == SeatType.Staff)
            {
                foreach(SeatInBuilding seatInBuilding in seat.GetHelpedSeats())
                {
                    if (seatInBuilding.WaitLineInBuilding.HadPassenger())
                    {
                        return seat;
                    }
                }
            }
        }
        if(!FirstLoad && IsBuilded) return null;
        //firstLoad = false;
        for (int i = 0; i < seats.Count; i++)
        {
            SeatInBuilding seat = seats[i];
            if (seat.isOpen && !seat.isSeatedIn && seat.SeatType == SeatType.Staff)
            {
                //Debug.Log(1);
                return seat;
            }
        }
        return null;
    }

 
    #region GetStat
    public float GetMoneyEarnedPerPassgenger()
    {
        float money = buildingSO.baseMoneyEarned;

        for (int i = 1; i < level; i++)
        {
            money *= (1 + buildingSO.baseMoneyEarnedIncreasePercentPerLevel / 100);
        }

        moneyEarnedPerPassenger = money;
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

        for (int i = 1; i < level; i++)
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
        CheckUnlockLevel();
    
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
            if (seat.SeatType == SeatType.Staff)
            {
                if (!seat.CheckStillHavePassengerInWait())
                {
                    //Debug.Log(gameObject.name);
                    //Debug.Log(seat.gameObject.name);
                    seat.Agent.OnFinishTask();
                }
                
            }
        }
       
    }

    public bool CheckNeedStaffHelp()
    {
        foreach (SeatInBuilding seat in seats)
        {
            if(seat.SeatType == SeatType.Staff && seat.isOpen && !seat.isSeatedIn)
            {
                if (seat.CheckStillHavePassengerInWait())
                {
                    return true;
                }
            }
        }

        return false;
    }

    void CheckUnlockLevel()
    {
        lockObject.SetActive(false);
        if (buildingSO.unlockedLevel > GameManager.Instance.UserData.level) lockObject.SetActive(true);
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
