using System;
using System.Collections.Generic;


using UnityEngine;

namespace Data
{
    [Serializable]
    public class UserData : SavePlayerPrefs
    {
        public bool alreadyLoad;
        public float money;
        public int currentExp;
        public int nextExp;
        public int level;
        public List<BuildedBuilding> buildedBuildingList = new List<BuildedBuilding>();   
        public Dictionary<int, int> buildedBuildingDict = new Dictionary<int, int>();

        public bool Init()
        {
            if (alreadyLoad)
            {
                ConvertListToDictionary();
                return false;
            }
            currentExp = 0;
            nextExp = 10;
            level = 1;
            alreadyLoad = true;

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
        public void AddExp(int exp, Action nextLevelAction)
        {
            this.currentExp += exp;
            CheckNextLevel(nextLevelAction);
        }

        public void CheckNextLevel(Action nextLevelAction)
        {
            if (currentExp >= nextExp)
            {
                level++;
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