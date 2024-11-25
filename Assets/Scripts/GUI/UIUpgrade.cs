using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] List<UpgradeSO> upgradeSOs = new List<UpgradeSO>();
    [SerializeField] UpgradeUI upgradeUIPref;
    List<UpgradeUI> upgradeUIs = new List<UpgradeUI>(); 

    [SerializeField] Transform viewParent;

    [SerializeField] Button backButton;

    bool firstLoad = true;

    private void Start()
    {
        upgradeSOs.Sort((a, b) => a.price.CompareTo(b.price));

        foreach (UpgradeSO upgradeSO in upgradeSOs)
        {
            if(GameManager.Instance.UserData.upgradeListIds.Contains(upgradeSO.id)) continue;
            UpgradeUI upgradeUI = Instantiate(upgradeUIPref, viewParent);
            upgradeUIs.Add(upgradeUI);
            upgradeUI.SetUp(upgradeSO);
        }
        CheckMoney();

        backButton.onClick.AddListener(BackButton);
    }

    public void CheckMoney()
    {
        foreach(UpgradeUI upgradeUI in upgradeUIs)
        {
            upgradeUI.CheckMoney();
        }

        if (firstLoad) Hide();
    }

    public override void Show()
    {
        base.Show();
        firstLoad = false;
        CheckMoney();
      
    }

    public void RemoveUpgradeUI(UpgradeUI upgradeUI)
    {
        upgradeUIs.Remove(upgradeUI);
    }
    public void BackButton()
    {
        Hide();
    }
}
