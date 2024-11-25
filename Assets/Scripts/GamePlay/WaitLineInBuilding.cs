using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitLineInBuilding : MonoBehaviour
{
    [SerializeField] SeatInBuilding seat;
    
    [SerializeField] List<Transform> waitPositions = new List<Transform>();
    [SerializeField] Queue<PassengerAgent> passengerAgents = new();
    int passengerCount = 0;

    bool firstLoad = true;
    public void OnStart()
    {
        seat.WaitLineInBuilding = this;
    }
    public void AddPassenger(PassengerAgent passengerAgent)
    {
        if(!CanAddMorePassenger()) return;

        CaculatePassenger(passengerAgent);
      
        passengerCount = passengerAgents.Count;
        //CaculateWaitPositions();
    }

    public void CaculateWaitPositions()
    {
        if (passengerAgents.Count <= 0)
        {
            return;
        }
        PassengerAgent firstPassenger = passengerAgents.Dequeue();

        int index = 0;
        foreach(PassengerAgent passengerAgent in passengerAgents)
        {
            Vector3 targetPosition = seat.transform.position;
            Vector3 direction = (targetPosition - new Vector3(passengerAgent.transform.position.x, targetPosition.y, passengerAgent.transform.position.z)).normalized;
            passengerAgent.SetAngle(direction);
            passengerAgent.SetDestination(waitPositions[index], true);
            index++;
        }

        firstPassenger.GoToSeatInBuilding(seat);
    }

    public bool CanAddMorePassenger()
    {
        if (passengerAgents.Count >= waitPositions.Count)
        {
            //Debug.Log("Full");
            return false;
        }
        return true;
    }

    public bool HadFreeSeat()
    {
        if(seat.isOpen && !seat.isSeatedIn)
        {
            return CanAddMorePassenger();
        }
        return false;
    }

    public bool HadStaff()
    {
        if(seat.hadStaffHelp) return true;
        return false;
    }
    void CaculatePassenger(PassengerAgent passengerAgent)
    {
        int i = passengerAgents.Count;
        if(i == 0)
        {
            if (seat.isOpen && !seat.isSeatedIn && seat.Agent == null)
            {
                //Debug.Log("First Passenger");
                passengerAgent.GoToSeatInBuilding(seat);
                return;
            }
        }
        passengerAgents.Enqueue(passengerAgent);
        Vector3 targetPosition = seat.transform.position;
        Vector3 direction = (targetPosition - new Vector3(passengerAgent.transform.position.x, targetPosition.y, passengerAgent.transform.position.z)).normalized;
        passengerAgent.SetAngle(direction);
        passengerAgent.SetDestination(waitPositions[i], true);

        firstLoad = false;
    }
    
    public bool HadPassenger()
    {
        if (!seat.isOpen) return true;
        if (passengerAgents.Count == 0) return false;
        return true;

    }

    public bool NeedStaffHelp()
    {
        if (!seat.isOpen) return true;

        return false;
    }
}
