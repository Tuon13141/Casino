using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatInBuilding : MonoBehaviour
{
    public bool isOpen = true;
    public SeatType SeatType = SeatType.Passenger;
    public bool isSeatedIn = false;
    public BuildingObject buildingObject;
    public bool hadStaffHelp = false;
    public Agent Agent;
    public WaitLineInBuilding WaitLineInBuilding { get; set; }
    [SerializeField] List<SeatInBuilding> HelpedSeats = new();
    public Vector3 agentAngle = new Vector3(0, 0, 0);

    private float serveTimer = 0f;

    [SerializeField] ProgressBox progressUI;
    ProgressBox currentProgressUI;

    public void SetUp(BuildingObject buildingObject)
    {
        this.buildingObject = buildingObject;
    }

    public Vector3 GetSeatTransform()
    {
        return transform.position;
    }

    public void OnUse()
    {
        isOpen = false;
    }

    public void OnSeated(Agent agent)
    {
        this.Agent = agent;
        switch (SeatType)
        {
            case SeatType.Passenger:
               
                StartPassengerSeat();
                break;
            case SeatType.Staff:
                OnStaffSeat();
                break;
        }
    }

    void OnStaffSeat()
    {
        isSeatedIn = true;
        foreach(SeatInBuilding seatInBuilding in HelpedSeats)
        {
            seatInBuilding.SetHadStaffHelp(true);
        }
    }

    public void SetHadStaffHelp(bool b)
    {
        hadStaffHelp = b;
    }

    public void SetStaffSeatHadStaffHelp(bool b)
    {
        foreach (SeatInBuilding seatInBuilding in HelpedSeats)
        {
            seatInBuilding.SetHadStaffHelp(b);
        }
    }

    void StartPassengerSeat()
    {
        isSeatedIn = true;
        serveTimer = buildingObject.BuildingSO.serveTime;
        if(currentProgressUI == null)
        {
            currentProgressUI = Instantiate(progressUI);

            currentProgressUI.SetUp(transform);
            currentProgressUI.SetActive(false);
        }
        else
        {
            currentProgressUI.SetProgress(0, 1);
        }

        Game.Update.AddTask(OnUpdate);
    }

    void OnUpdate()
    {
      
        if (!buildingObject.BuildingSO.needStaffHelp || hadStaffHelp && isSeatedIn)
        {   
            HandlePassengerServing();
        }
    }

    void HandlePassengerServing()
    {
        if (serveTimer > 0)
        {
            currentProgressUI.SetActive(true);

            currentProgressUI.SetProgress(serveTimer, buildingObject.BuildingSO.serveTime);
            serveTimer -= Time.deltaTime;
            return;
        }

        // Calculate money and experience
        float moneyEarned = buildingObject.BuildingSO.baseMoneyEarned;
        float expEarned = buildingObject.BuildingSO.baseExpEarned;

        for (int i = 1; i < buildingObject.level; i++)
        {
            moneyEarned *= (1 + buildingObject.BuildingSO.baseMoneyEarnedIncreasePercentPerLevel / 100);
            expEarned += (1 + buildingObject.BuildingSO.baseExpEarnedIncreasePercentPerLevel / 100);
        }

        // Add the earned resources
        GameManager.Instance.AddMoney(moneyEarned);
        GameManager.Instance.AddExp(expEarned);

        // Finish the task and reset
        Agent.OnFinishTask();
        isSeatedIn = false;
        Agent = null;
        currentProgressUI.SetActive(false);

        // Reset the seat and wait line
        isOpen = true;

        WaitLineInBuilding.CaculateWaitPositions();

        buildingObject.CheckRemainSeatPassengerToFreeStaff();

        Game.Update.RemoveTask(OnUpdate);
    }
    
    public bool CheckStillHavePassengerInWait()
    {
        if(Agent ==  null) return true;
        foreach(SeatInBuilding seat in HelpedSeats)
        {
            if(seat.WaitLineInBuilding.HadPassenger()) return true;
        }

        return false;
    }
}

public enum SeatType
{
    Passenger, Staff
}
