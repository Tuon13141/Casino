using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerManager : Singleton<PassengerManager>
{
    float passengerCooldown = 0;
    public bool canSpawnPassenger = false;
    [SerializeField] PassengerAgent passengerPref;
    [SerializeField] List<Transform> startPoints = new List<Transform>();
    [SerializeField] List<PassengerAgent> passengerAgents = new List<PassengerAgent>();
    public Transform endPoint;
    [SerializeField] Transform parent;

    float timer = 0;
    public float moveSpeed = 10;
    private void Start()
    {
        passengerCooldown = GameManager.Instance.UserData.passengerCooldown;
        timer = passengerCooldown - 5;
        Game.Update.AddTask(OnUpdate);
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (timer > passengerCooldown)
        {
            SpawnPassenger();
            timer = 0;
        }
    }

    void SpawnPassenger()
    {
        if(!canSpawnPassenger) return;
        BuildingObject receptionistArea = BuildingManager.Instance.GetRandomReceptionistAreaForPassenger();
        if(receptionistArea == null) return;

        PassengerAgent passengerAgent = Instantiate(passengerPref);
        passengerAgent.gameObject.transform.position = startPoints[Random.Range(0, startPoints.Count)].position;

        passengerAgent.gameObject.transform.parent = parent;
        passengerAgent.OnStart();

        
        List<BuildingObject> buildingObjects = new List<BuildingObject>();
        BuildingObject buildingObject = BuildingManager.Instance.GetRandomBuildingForPassenger();

        if (buildingObject != null)
        {
            buildingObjects.Add(buildingObject);
        }
        passengerAgent.GetTask(receptionistArea, buildingObjects);

        passengerAgents.Add(passengerAgent);
    }

    public void SetNewPassengerCooldown()
    {
        passengerCooldown = GameManager.Instance.UserData.passengerCooldown;
    }

    public void RemovePassengerAgent(PassengerAgent passengerAgent) { passengerAgents.Remove(passengerAgent);}

    public void SetTempPassengerMoveSpeed(float speed) { foreach(PassengerAgent passengerAgent in passengerAgents) { passengerAgent.speed = speed; } }

    public void ResetSpeed() { foreach (PassengerAgent passengerAgent in passengerAgents) { passengerAgent.speed = moveSpeed; } }
}
