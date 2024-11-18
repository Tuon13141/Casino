using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool isOpen = true;
    public SeatType SeatType = SeatType.Passenger;
    public bool isUsed = false;

    public Vector3 GetSeatTransform()
    {
        return transform.position;
    }
}

public enum SeatType
{
    Passenger, Staff
}
