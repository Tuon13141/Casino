using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
using Data;
using Unity.AI.Navigation;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] NavMeshSurface navMeshSurface;
    public UserData UserData
    {
        get; private set;
    }
    protected override void Awake()
    {
        base.Awake();
        Game.Launch();
        UserData = Game.Data.Load<UserData>();
        if (UserData.Init())
        {
            //BuildingManager.Instance.NeedTutorial = true;
        }
        //UserData.coin = 0;
        BakeNavMesh();
    }
   

    private void Start()
    {
        ChangeState(GameStates.Start);
        GameUI.Instance.Get<UIInGame>().Show();
    }

   
    [SerializeField] private GameStates _state = GameStates.Retry;
    public void ChangeState(GameStates newState)
    {
        if (newState == _state) return;
 
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

    public void AddBuildedBuilding(float cost, int id)
    {
        if (CheckBuildedBuiling(id) != -1) return;
        UserData.money -= cost;
        UserData.AddBuildedBuilding(id, 1);
        GameUI.Instance.Get<UIInGame>().SetCoinText(UserData.money);
        BakeNavMesh();
    }
    public void UpdateBuilding(float cost, int id)
    {
        if (CheckBuildedBuiling(id) == -1) return;
        AddMoney(-cost);
        UserData.UpdateBuildedBuilding(id);
        
        BakeNavMesh();
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
    public void AddExp(float exp)
    {
        UserData.AddExp(exp, BuildingManager.Instance.CheckBuildingUpdate);
        GameUI.Instance.Get<UIInGame>().SetLevelProgress(UserData.currentExp, UserData.nextExp, UserData.startExp, UserData.level);
    }

    public void AddMoney(float money)
    {
        UserData.money += money;
        BuildingManager.Instance.CheckBuildingUpdate();
        GameUI.Instance.Get<UIInGame>().SetCoinText(UserData.money);
        GameUI.Instance.Get<UIUpdateBuilding>().CheckMoneyToUpdate();
    }


    public void BakeNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}

public enum GameStates
{
    Play, Win, Lose, Home, Tutorial, Start, Retry, NextLevel
}
