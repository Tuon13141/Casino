using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingSO : ScriptableObject
{
    public BuildingType buildingType;
    public GameObject buildingPref;
    public int unlockedLevel = 1;
    public float buildCost;
    public float baseMoneyEarned;
    public float baseExpEarned; 
    public float baseUpdateCost;
    public float baseMoneyEarnedIncreasePercentPerLevel = 10;
    public float baseExpEarnedIncreasePercentPerLevel = 10;
    public float baseMoneyUpdateIncreasePercentPerLevel = 100;
    public bool needStaffHelp = false;
    public float serveTime = 2f;
}

public enum BuildingType
{
    PokerTable, CasinoMachine, BidaTable
}
