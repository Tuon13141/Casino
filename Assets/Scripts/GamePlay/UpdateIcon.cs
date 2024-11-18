using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateIcon : MonoBehaviour
{
    BuildingObject buildingObject;

    public void SetUp(BuildingObject buildingObject)
    {
        this.buildingObject = buildingObject;
    }
    public void OnClick()
    {
        if (!buildingObject.IsBuilded)
        {
            GameUI.Instance.Get<UIOpenBuilding>().Show();
            GameUI.Instance.Get<UIOpenBuilding>().SetUp(buildingObject.GetBuildCost(), buildingObject.GetMoneyEarnedPerPassgenger(), buildingObject.Build);
        }
        else
        {

        }   

        Debug.Log(gameObject.name + " was clicked!");
    }
}
