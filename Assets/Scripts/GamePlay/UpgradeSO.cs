using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UpgradeSO : ScriptableObject
{
    public string id;
    public string title;
    public string description;
    public UpgradeType upgradeType;
    public Sprite icon;
    public float price;
    public float amount;

    public void Upgrade()
    {
        GameManager.Instance.Upgrade(this);
    }
}

public enum UpgradeType
{
    Staff, Passenger, Speed, Money
}
