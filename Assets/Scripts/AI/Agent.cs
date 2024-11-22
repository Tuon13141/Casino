using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [SerializeField] float baseMoveSpeed;
    float moveSpeed;
    [SerializeField] Transform destination; 
    public float speed = 10f; 

    private NavMeshPath navMeshPath; 
    protected List<Vector3> movePath = new List<Vector3>(); 
    protected int currentIndex;
    protected bool isMoving = false; 
    protected bool hadReachTarget = false;


    protected SeatInBuilding seat;
    [SerializeField] float rotationSpeed = 15f;
    bool isWait = false;
    Vector3 angle;

    public virtual void OnStart()
    {
        navMeshPath = new NavMeshPath();
        ResetMovement();
    }

    public virtual void OnUpdate()
    {
        if (isMoving)
        {
            MoveAlongPath();
        }
        else
        {
            if (isWait)
            {
                Game.Update.AddTask(RotateAgent);
            }
            Game.Update.RemoveTask(OnUpdate);
        }
    }

    protected virtual void MoveAlongPath()
    {
        if (currentIndex >= movePath.Count) 
        {
            OnDestinationReached();
            isMoving = false;
            hadReachTarget = true;
            return;
        }

        Vector3 targetPosition = movePath[currentIndex];
        Vector3 direction = (targetPosition - new Vector3(transform.position.x, targetPosition.y, transform.position.z)).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentIndex++;
        }
    }

    public void SetDestination(Transform destination, bool isWait = false)
    {
        this.destination = destination;
        this.isWait = isWait;
        ResetMovement();
        if(seat != null) seat.OnUse();

        if (NavMesh.CalculatePath(transform.position, destination.position, NavMesh.AllAreas, navMeshPath))
        {
            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                movePath.AddRange(navMeshPath.corners);
                currentIndex = 0;
                isMoving = true;
                hadReachTarget = false;
            }
            else
            {
                Debug.Log("Can't 2");
            }
        }
        else
        {
            Debug.Log("Can't 1");
        }

        Game.Update.AddTask(OnUpdate);
    }

    protected virtual void OnDestinationReached()
    {
        if(seat == null) return;
        seat.OnSeated(this);

        angle = seat.agentAngle;
        Game.Update.AddTask(RotateAgent);
    }

    void RotateAgent()
    {
        Quaternion targetRotation = Quaternion.Euler(angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) <= 0)
        {
            isWait = false;
            Game.Update.RemoveTask(RotateAgent);
        }
    }


    public virtual void OnFinishTask()
    {

    }

    private void ResetMovement()
    {
        movePath.Clear();
        currentIndex = 0;
        isMoving = false;
        hadReachTarget = false;
    }

    public void SetSeat(SeatInBuilding seat)
    {
        this.seat = seat;
    }

    public void SetAngle(Vector3 vector3)
    {
        float angle = Mathf.Atan2(vector3.x, vector3.z) * Mathf.Rad2Deg;
        this.angle = new Vector3(0,  angle, 0);
    }
    private void OnDestroy()
    {
        Game.Update.RemoveTask(RotateAgent);
    }
}

