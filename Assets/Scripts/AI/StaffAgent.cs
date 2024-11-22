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
    private float originalSpeed; 
    private float speedUpTimer = 0f; 
    private bool isSpeedingUp = false;
    public override void OnStart()
    {
        base.OnStart();
        originalSpeed = speed;
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
            case StaffState.Busy:
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
            ChangeState(StaffState.Busy);
            Game.Update.AddTask(OnStaffReachedSeat);
        }
     
    }

    private void OnStaffReachedSeat()
    {
        if(hadReachTarget && seat != null)
        {
            //Debug.Log("Agent has arrived at the destination!");
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
        seat = null;
        ChangeState(StaffState.Free);
    }

    protected override void OnDestinationReached()
    {
        base.OnDestinationReached();
        seat.SetHadStaffHelp(true);
    }

    public void SpeedUpTemporary(float time, float index)
    {
        if (BuildingManager.Instance.NeedTutorial)
        {
            BuildingManager.Instance.DestroyTutorialPointer();
        }
        speed = originalSpeed * index;
        speedUpTimer = time; 
        isSpeedingUp = true;

        Game.Update.AddTask(OnSpeedUp);
    }

    void OnSpeedUp()
    {
        if (isSpeedingUp)
        {
            speedUpTimer -= Time.deltaTime;
            if (speedUpTimer <= 0f) 
            {
                speed = originalSpeed;
                isSpeedingUp = false; 
            }
        }
        else
        {
            Game.Update.RemoveTask(OnSpeedUp);
        }
     
    }

    protected override void MoveAlongPath()
    {
        if(BuildingManager.Instance.NeedTutorialPointer && currentIndex >= movePath.Count / 2 )
        {
            BuildingManager.Instance.PlaceTutorialPointer(transform);
            speed = 0;
        }
        base.MoveAlongPath();
    }
}
public enum StaffType
{
    AllPosition
}

public enum StaffState
{
    Free, OnGoingToDestination, Busy
}
