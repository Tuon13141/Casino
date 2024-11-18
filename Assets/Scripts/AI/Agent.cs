using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [SerializeField] float baseMoveSpeed;
    float moveSpeed;
    [SerializeField] Vector3 destination;

    protected NavMeshAgent navMeshAgent;

    public virtual void OnStart()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        navMeshAgent.destination = destination;
    }
}

