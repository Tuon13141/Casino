using System;
using System.Collections.Generic;


using UnityEngine;

namespace Data
{
    [Serializable]
    public class UserData : SavePlayerPrefs
    {
        public bool alreadyLoad;
        public bool needTutorial;
        public float money;
        public float startExp;
        public float currentExp;
        public float nextExp;
        public int level;
        public int numberOfStaff;
        public float passengerCooldown;
        public float bonusIncome;
        public float bonusSpeed;

        public List<BuildedBuilding> buildedBuildingList = new List<BuildedBuilding>();   
        public Dictionary<int, int> buildedBuildingDict = new Dictionary<int, int>();

        public List<string> upgradeListIds = new List<string>();
        public bool Init()
        {
            if (alreadyLoad)
            {
                ConvertListToDictionary();
                return false;
            }
            money = 15;
            currentExp = 0;
            startExp = 0;
            nextExp = 10;
            level = 1;
            bonusIncome = 1;
            bonusSpeed = 1;
            alreadyLoad = true;
            numberOfStaff = 1;
            passengerCooldown = 10f;
            needTutorial = true;
            return true;
        }

        public void AddBuildedBuilding(int id, int level)
        {
            buildedBuildingList.Add(new BuildedBuilding(id, level));
            buildedBuildingDict.Add(id, level);
        }

        public void UpdateBuildedBuilding(int id)
        {
            if (buildedBuildingDict.ContainsKey(id))
            {
                buildedBuildingDict[id]++;
                buildedBuildingList.Find(building => building.id == id).level++;

            }
            else
            {
                AddBuildedBuilding(id, 1);
            }
        }
        public void AddExp(float exp, Action nextLevelAction)
        {
            this.currentExp += exp;
            CheckNextLevel(nextLevelAction);
        }

        public void CheckNextLevel(Action nextLevelAction)
        {
            if (currentExp >= nextExp)
            {
                level++;
                startExp = nextExp;
                nextExp = nextExp * 2;
              
                nextLevelAction?.Invoke();
            }
        }

        public void ConvertListToDictionary()
        {
            buildedBuildingDict.Clear(); 
            foreach (var building in buildedBuildingList)
            {
                if (!buildedBuildingDict.ContainsKey(building.id))
                {
                    buildedBuildingDict.Add(building.id, building.level);
                }
                else
                {
                    buildedBuildingDict[building.id] = building.level; 
                }
            }
        }
    }

    [Serializable]
    public class BuildedBuilding
    {
        public int id;
        public int level;

        public BuildedBuilding(int id, int level)
        {
            this.id = id;
            this.level = level;
        }

        public override bool Equals(object obj)
        {
            if (obj is BuildedBuilding other)
            {
                return this.id == other.id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}