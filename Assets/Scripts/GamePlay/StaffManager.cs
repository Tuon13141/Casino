using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : Singleton<StaffManager>
{
    int staffCount = 0;
    [SerializeField] Transform staffParent;
    [SerializeField] StaffAgent staffPref;        
    public List<Transform> startPoints = new List<Transform>();
    [SerializeField] List<StaffAgent> staffAgents = new List<StaffAgent>();

    int startPointIndex = 0;
    public float moveSpeed = 3;

    private void Start()
    {
        staffCount = GameManager.Instance.UserData.numberOfStaff;

        for (int i = 0; i < staffCount; i++)
        {
            if (i >= startPoints.Count) break ;

            StaffAgent agent = Instantiate(staffPref);
            agent.transform.position = startPoints[i].position;
            staffAgents.Add(agent);
            agent.gameObject.transform.SetParent(staffParent);
            
            agent.StartPosition = startPoints[i];
            agent.OnStart();

            startPointIndex = i;
        }

        Game.Update.AddTask(OnUpdate);
    }

    public void OnUpdate()
    {
        for (int i = staffAgents.Count - 1; i >= 0; i--)
        {
            StaffAgent agent = staffAgents[i];
            if (agent.StaffState != StaffState.Free) continue;

            switch (agent.StaffType)
            {
                case StaffType.AllPosition:
                    BuildingObject buildingObject = BuildingManager.Instance.GetNeedStaffHelpBuilding();
                   
                    if (buildingObject == null) break;
                    agent.GetTask(buildingObject);
                 
                    break;
            }
        }

        BuildingManager.Instance.FirstLoad = false;
    }

    public void AddStaff()
    {
        if(staffCount >= startPoints.Count) return;

        StaffAgent agent = Instantiate(staffPref);
        agent.transform.position = startPoints[startPointIndex].position;
        staffAgents.Add(agent);
        agent.gameObject.transform.SetParent(staffParent);

        agent.StartPosition = startPoints[startPointIndex];
        agent.OnStart();
    }
}
