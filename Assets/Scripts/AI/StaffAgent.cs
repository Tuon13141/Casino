using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StaffAgent : Agent
{
    [SerializeField] StaffState staffState = StaffState.Free;
    [SerializeField] StaffType staffType = StaffType.AllPosition;
    public StaffType StaffType => staffType;
    public StaffState StaffState => staffState;
    public Transform StartPosition;

    public override void OnStart()
    {
        base.OnStart();
        
    }

    public void ChangeState(StaffState newState)
    {
        if (newState == staffState) return;

        staffState = newState;
        EnterNewState();
    }

    private void EnterNewState()
    {
        switch (staffState)
        {
            case StaffState.Free:
                OnFree();
                break;
            case StaffState.OnGoingToDestination:
                break;
   
            default:
                break;
        }
    }

    void OnFree()
    {
        SetDestination(StartPosition);
    }

    public void GetTask(BuildingObject buildingObject)
    {
        SetSeat(buildingObject.GetAvailableSeatForStaff());
        //Debug.Log(seat);
        if (seat != null)
        {
            SetDestination(seat.transform);
            Game.Update.AddTask(OnStaffReachedSeat);
        }
     
    }

    private void OnStaffReachedSeat()
    {
        if(hadReachTarget && seat != null)
        {
            Debug.Log("Agent has arrived at the destination!");
            OnDestinationReached();
            Game.Update.RemoveTask(OnStaffReachedSeat);
        }    
          
    }

    public override void OnFinishTask()
    {
        base.OnFinishTask();
        seat.isSeatedIn = false;
        seat.isOpen = true;
        seat.SetStaffSeatHadStaffHelp(false);
        ChangeState(StaffState.Free);
    }

    protected override void OnDestinationReached()
    {
        base.OnDestinationReached();
        seat.SetHadStaffHelp(true);
    }
}
public enum StaffType
{
    AllPosition
}

public enum StaffState
{
    Free, OnGoingToDestination, OnDestination
}
