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
    public WaitLineInBuilding WaitLineInBuilding { get;set; }
    [SerializeField] SeatInBuilding staffHelpSeat;
    public Vector3 agentAngle = new Vector3(0, 0 , 0);
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
        //isSeatedIn = true;
    }

    public void OnSeated(Agent agent)
    {
        this.Agent = agent;
        switch (SeatType)
        {
            case SeatType.Passenger:
                isSeatedIn = true;
                StartCoroutine(OnPassengerSeat());
                break;
            case SeatType.Staff:
                OnStaffSeat();
                break;
        }
    }

    void OnStaffSeat()
    {
        isSeatedIn = true;
        staffHelpSeat.SetHadStaffHelp(true);
    }
    public void SetHadStaffHelp(bool b)
    {
        hadStaffHelp = b;
    }

    public void SetStaffSeatHadStaffHelp(bool b)
    {
        staffHelpSeat.SetHadStaffHelp(b);
    }
    IEnumerator OnPassengerSeat()
    {
        if (buildingObject.BuildingSO.needStaffHelp)
        {
            yield return new WaitUntil(() => hadStaffHelp);
        }
       

        yield return new WaitForSeconds(buildingObject.BuildingSO.serveTime);

        float moneyEarned = buildingObject.BuildingSO.baseMoneyEarned;
        float expEarned = buildingObject.BuildingSO.baseExpEarned;
        for (int i = 1; i < buildingObject.level; i++)
        {
            moneyEarned *= (1 + buildingObject.BuildingSO.baseMoneyEarnedIncreasePercentPerLevel / 100);
            expEarned += (1 + buildingObject.BuildingSO.baseExpEarnedIncreasePercentPerLevel / 100);
        }
        GameManager.Instance.AddMoney(moneyEarned);
        GameManager.Instance.AddExp(expEarned);
        Agent.OnFinishTask();
        isSeatedIn = false;
        Agent = null;
        yield return new WaitForSeconds(0.5f);
        isOpen = true;
        WaitLineInBuilding.CaculateWaitPositions();
        buildingObject.CheckRemainSeatPassengerToFreeStaff();
    }
}

public enum SeatType
{
    Passenger, Staff
}
