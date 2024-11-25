using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI priceText;

    [SerializeField] float price;
    UpgradeSO upgradeSO;

    [SerializeField] Sprite offButton;
    [SerializeField] Sprite onButton;

    [SerializeField] Image buttonImage;
    [SerializeField] Button button;
    public void SetUp(UpgradeSO upgradeSO)
    {
        icon.sprite = upgradeSO.icon;
        title.text = upgradeSO.title;
        description.text = upgradeSO.description;
        priceText.text = upgradeSO.price.ToString();
        price = upgradeSO.price;
        this.upgradeSO = upgradeSO;
        button.onClick.AddListener(OnButtonClick);
    }

    public void CheckMoney()
    {
        if (GameManager.Instance.UserData.money < price)
        {
            buttonImage.sprite = offButton;
            button.SetActive(false);
        }
        else
        {
            buttonImage.sprite = onButton;
            button.SetActive(true);
        }
    }

    public void OnButtonClick()
    {
        upgradeSO.Upgrade();
        GameUI.Instance.Get<UIUpgrade>().RemoveUpgradeUI(this);
        Destroy(this.gameObject);
    }
}
