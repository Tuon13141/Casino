using Data;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public int ID = -1;
    public int level = 1;

    [SerializeField] BuildingSO buildingSO;
    List<Seat> seats = new List<Seat>();
    public bool IsBuilded = false;
    public float moneyEarnedPerPassenger;

    [SerializeField] UpdateIcon updateIcon;
    GameObject currentBuildingObject;
    [SerializeField] List<GameObject> destroyAfterBuildObjects = new List<GameObject>();

    private void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        moneyEarnedPerPassenger = GetMoneyEarnedPerPassgenger();
        SetUpUpdateIcon();
        
        int level = GameManager.Instance.CheckBuildedBuiling(ID);
        if(level != -1)
        {
            this.level = level;
            IsBuilded = true;
            SpawnBuildingPref();
        }

        CheckUpdate();
    }

    public void SetUpUpdateIcon()
    {
        updateIcon.SetUp(this);
    }

    void SpawnBuildingPref()
    {
        currentBuildingObject = Instantiate(buildingSO.buildingPref, transform);
        IsBuilded = true;
        foreach (GameObject go in destroyAfterBuildObjects)
        {
            Destroy(go);
        }

        Seat[] seats = currentBuildingObject.GetComponentsInChildren<Seat>();
        this.seats.Clear();
        this.seats.AddRange(seats);
    }
    public void Build()
    {
        SpawnBuildingPref();

        GameManager.Instance.AddBuildedBuilding(buildingSO.buildCost, ID);
    }

    public Seat GetAvailableSeatForPassenger()
    {
        foreach(Seat seat in this.seats)
        {
            if(seat.isOpen && !seat.isUsed && seat.SeatType == SeatType.Passenger)
            {
                return seat;
            }
        }
        return null;
    }

    #region GetStat
    public float GetMoneyEarnedPerPassgenger()
    {
        float money = buildingSO.baseMoneyEarned;

        for (int i = 0; i < level; i++)
        {
            money *= (1 + buildingSO.baseMoneyEarnedIncreasePercentPerLevel / 100);
        }

        return money;
    }
    public float GetNextMoneyEarnedPerPassgenger()
    {
        float money = moneyEarnedPerPassenger;

        money *= (1 + buildingSO.baseMoneyEarnedIncreasePercentPerLevel / 100);

        return money;
    }
    public float GetNextUpdateCost()
    {
        float money = buildingSO.baseUpdateCost;

        for (int i = 0; i < level; i++)
        {
            money *= (1 + buildingSO.baseMoneyUpdateIncreasePercentPerLevel / 100);
        }

        return money;
    }
    public float GetBuildCost()
    {
        return buildingSO.buildCost;
    }
    public void CheckUpdate()
    {
        updateIcon.gameObject.SetActive(false);
        UserData userData = GameManager.Instance.UserData;
        if (userData.level >= buildingSO.unlockedLevel)
        {
            if(!IsBuilded)
            {
                if(userData.money >= buildingSO.buildCost)
                {
                    updateIcon.gameObject.SetActive(true);
                }
            }
            else
            {
                if (userData.money >= GetNextUpdateCost())
                {
                    updateIcon.gameObject.SetActive(true);
                }
            }
            
        }
    }
    #endregion
}
