using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StaffAgent : Agent
{
    [SerializeField] StaffState staffState = StaffState.Free;
    [SerializeField] StaffType staffType = StaffType.AllPosition;
    public StaffType StaffType => StaffType;
    public Vector3 StartPosition;

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
        //StaffManager.Instance.FindTask(this);
        SetDestination(StartPosition);
    }

    public void GetTask(BuildingObject buildingObject)
    {
        SeatInBuilding seat = buildingObject.GetAvailableSeatForStaff();
        SetDestination(seat.transform.position);
        seat.OnUse();
        
    }

    private IEnumerator WaitForAgentArrival(SeatInBuilding seat)
    {
        yield return new WaitUntil(() =>
            !navMeshAgent.pathPending &&
            navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
            (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
        );

        Debug.Log("Agent has arrived at the destination!");
        OnDestinationReached(seat);
    }

    private void OnDestinationReached(SeatInBuilding seat)
    {
        seat.OnSeated();
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
