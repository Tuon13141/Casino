using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatInBuilding : MonoBehaviour
{
    public bool isOpen = true;
    public SeatType SeatType = SeatType.Passenger;
    public bool isSeatedIn = false;
    BuildingObject buildingObject;

    public Agent Agent;
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

    public void OnSeated()
    {
        switch (SeatType)
        {
            case SeatType.Passenger:
                
                break;
            case SeatType.Staff:
                OnStaffSeat();
                break;
        }
    }

    void OnStaffSeat()
    {
        buildingObject.SetHadStaffHelp(true);
    }

    IEnumerator OnPassengerSeat()
    {
        yield return new WaitUntil(() => buildingObject.hadStaffHelp);

        yield return new WaitForSeconds(buildingObject.BuildingSO.serveTime);

        GameManager.Instance.AddMoney(0);
        buildingObject.CheckRemainSeatPassengerToFreeStaff();
    }
}

public enum SeatType
{
    Passenger, Staff
}
