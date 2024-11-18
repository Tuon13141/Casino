using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
using Data;

public class GameManager : Singleton<GameManager>
{
    public UserData UserData
    {
        get; private set;
    }
    protected override void Awake()
    {
        base.Awake();
        Game.Launch();
        UserData = Game.Data.Load<UserData>();
        UserData.Init();
        //UserData.coin = 0;
    }
   

    private void Start()
    {
        ChangeState(GameStates.Start);
    }

   
    [SerializeField] private GameStates _state = GameStates.Retry;
    public void ChangeState(GameStates newState)
    {
        if (newState == _state) return;
        ExitCurrentState();
        _state = newState;
        EnterNewState();
    }

    private void EnterNewState()
    {

        switch (_state)
        {
            case GameStates.Tutorial:
               
                break;
            case GameStates.Home:
                break;
            case GameStates.Start:
         
                break;
            case GameStates.Play:
              
                break;
            case GameStates.Retry:

                break;
            case GameStates.Win:
            
                break;
            case GameStates.Lose:

                break;
            case GameStates.NextLevel:
       
                break;
            default:
                break;
        }
    }

    private void ExitCurrentState()
    {
        switch (_state)
        {
            case GameStates.Tutorial:
                break;
            case GameStates.Home:
                break;
            case GameStates.Start:
                break;
            case GameStates.Play:
   
                break;
            case GameStates.Retry:
                break;
            case GameStates.Win:
                break;
            case GameStates.Lose:
                break;
            case GameStates.NextLevel:
          
                break;
            default:
                break;
        }
    }

    public void AddBuildedBuilding(float cost, int id)
    {
        if (CheckBuildedBuiling(id) != -1) return;
        UserData.money -= cost;
        UserData.AddBuildedBuilding(id, 1);       
    }
    public void UpdateBuilding(float cost, int id)
    {
        if (CheckBuildedBuiling(id) == -1) return;
        UserData.money -= cost;
        UserData.UpdateBuildedBuilding(id);
    }
    public int CheckBuildedBuiling(int id)
    {
        if (UserData.buildedBuildingDict.ContainsKey(id))
        {
            return UserData.buildedBuildingDict[id];
        }

        return -1;
    }

    void GetBuildingDict()
    {
        foreach(int key in UserData.buildedBuildingDict.Keys)
        {
            Debug.Log(key);
        }
    }
    public void AddExp(int exp)
    {
        UserData.AddExp(exp, BuildingManager.Instance.CheckBuildingUpdate);
    }

    public void AddMoney(int money)
    {
        UserData.money += money;
        BuildingManager.Instance.CheckBuildingUpdate();
    }

  
}

public enum GameStates
{
    Play, Win, Lose, Home, Tutorial, Start, Retry, NextLevel
}
