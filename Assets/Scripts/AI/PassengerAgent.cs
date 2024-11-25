using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerAgent : Agent
{
    [SerializeField] PassengerState passengerState = PassengerState.Free;

    BuildingObject receptionistArea;
    List<BuildingObject> destinationBuildings = new();

    int buildingIndex = 0;
    bool nextBuilding = true;
    public override void OnStart()
    {
        base.OnStart();
        speed = PassengerManager.Instance.moveSpeed;
    }

    public void GetTask(BuildingObject receptionistArea, List<BuildingObject> buildingObjects)
    {
        this.receptionistArea = receptionistArea;
        destinationBuildings.AddRange(buildingObjects);
        ChangeState(PassengerState.OnGoingToReceptionistArea);
    }

    public void GoToSeatInBuilding(SeatInBuilding seat)
    {
        this.seat = seat; 
        SetDestination(seat.transform);
        //Game.Update.AddTask(OnFinishTask);
    }

    protected override void OnDestinationReached()
    {
        base.OnDestinationReached();
    }
    public override void OnFinishTask()
    {
        base.OnFinishTask();
        if (hadReachTarget )
        {
            if (passengerState == PassengerState.OnGoingToReceptionistArea)
            {
                //Debug.Log("Get Ticket");
                seat = null;
                ChangeState(PassengerState.OnGoingToMainDestination);
            }
            else if (passengerState == PassengerState.OnGoingToMainDestination)
            {
                if (buildingIndex >= destinationBuildings.Count)
                {
                    //Debug.Log("Done");
                    ChangeState(PassengerState.OnGoingOut);
                }
                else
                {
                    ChangeState(PassengerState.OnGoingToMainDestination);
                }
               
            }
            else
            {
                
            }
            //Game.Update.RemoveTask(OnFinishTask);
        }

    }
    public void ChangeState(PassengerState newState)
    {
        if (newState == passengerState) return;

        passengerState = newState;
        EnterNewState();
    }

    private void EnterNewState()
    {
        switch (passengerState)
        {
            case PassengerState.OnGoingToReceptionistArea:
                receptionistArea.GetAvailableSeatForPassenger(this);
                break;
            case PassengerState.OnGoingToMainDestination:
                GoToMainDestination();
                break;
            case PassengerState.OnGoingOut:
                OnGoingOut();
                break;
            case PassengerState.OnBuilding:

                break;
            default:
                break;
        }
    }
    void GoToMainDestination()
    {
        if (!destinationBuildings[buildingIndex].GetAvailableSeatForPassenger(this))
        {
            //Debug.Log(1);
            ChangeState(PassengerState.OnGoingOut);
            return;
        }
        //SetDestination();
        buildingIndex++;
        if (seat == null)
        {
            //ChangeState(PassengerState.OnGoingOut);
            return;
        }
        seat.OnUse(this);
      
        //Game.Update.AddTask(OnFinishTask);

    }
    void OnGoingOut()
    {
        seat = null;
        SetDestination(PassengerManager.Instance.endPoint);
     
        Game.Update.AddTask(WaitForOutOfMap);
    }

    private void WaitForOutOfMap()
    {
        if (hadReachTarget)
        {
            Game.Update.RemoveTask(WaitForOutOfMap);
            PassengerManager.Instance.RemovePassengerAgent(this);
            Destroy(gameObject);
        }

    }
}

public enum PassengerState
{
    OnGoingToMainDestination, OnGoingOut, OnBuilding, OnGoingToReceptionistArea, Free
}
