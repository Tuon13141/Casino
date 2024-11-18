using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerAgent : Agent
{
    [SerializeField] PassengerState passengerState;

    public override void OnStart()
    {
        base.OnStart();

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
            case PassengerState.OnGoingToMainDestination:
    ;
                break;
            case PassengerState.OnGoingOut:
                break;

            default:
                break;
        }
    }
}

public enum PassengerState
{
    OnGoingToMainDestination, OnGoingOut
}
