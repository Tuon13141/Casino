using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdateBuilding : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] Button okButton;
    [SerializeField] Button backButton;

    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI earnText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI nextLevelText;

    Action onOkButton;

    float cost;
    public void SetUp(float cost, float earn, Action onOkButton, int level)
    {
        this.cost = cost;
        costText.text = cost.ToString();
        earnText.text = earn.ToString();
        levelText.text = "Level " + level.ToString();
        nextLevelText.text = "Next Level : " + (level + 1).ToString();

        this.onOkButton = onOkButton;

        CheckMoneyToUpdate();
    }

    Action onHideAction;
    public void OkButton()
    {
        onOkButton?.Invoke();
        onHideAction?.Invoke();
    }

    public void BackButton()
    {
        
        Hide();
    }

    private void Start()
    {
        okButton.onClick.AddListener(OkButton);
        backButton.onClick.AddListener(BackButton);
    }

    public void CheckMoneyToUpdate()
    {
        if (GameManager.Instance.UserData.money < cost)
        {
            okButton.gameObject.SetActive(false);
        }
        else
        {
            okButton.gameObject.SetActive(true);
        }
    }
}

