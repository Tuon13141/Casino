using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : Singleton<StaffManager>
{
    int staffCount = 0;
    Transform staffParent;
    [SerializeField] StaffAgent staffPref;        
    public List<Vector3> startPoints = new List<Vector3>();
    [SerializeField] List<StaffAgent> staffAgents = new List<StaffAgent>();
    private void Start()
    {
        for (int i = 0; i < staffCount; i++)
        {
            StaffAgent agent = Instantiate(staffPref);
            agent.transform.position = startPoints[i];
            staffAgents.Add(agent);
            agent.gameObject.transform.SetParent(staffParent);
            
            agent.StartPosition = startPoints[i];
            agent.OnStart();
        }

        Game.Update.AddTask(OnUpdate);
    }

    public void OnUpdate()
    {
        foreach (StaffAgent agent in staffAgents)
        {
            switch (agent.StaffType)
            {
                case StaffType.AllPosition:
                    BuildingObject buildingObject = BuildingManager.Instance.GetNeedStaffHelpBuilding();
                    if (buildingObject == null) break;
                    agent.GetTask(buildingObject);
                    break;
            }
        }
        
    }
}
