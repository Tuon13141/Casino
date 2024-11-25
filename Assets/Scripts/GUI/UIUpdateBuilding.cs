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

    bool firstLoad = true;

    Action onOkButton;

    float cost;

    [SerializeField] RectTransform board;
    public void SetUp(float cost, float earn, Action onOkButton, int level, Transform transform)
    {
        this.cost = cost;
        costText.text = cost.ToString();
        earnText.text = earn.ToString();
        levelText.text = "Level " + level.ToString();
        nextLevelText.text = "Next Level : " + (level + 1).ToString();

        this.onOkButton = onOkButton;

        CheckMoneyToUpdate();

        Vector3 worldPos = transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPos);
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
      
        Vector3 midPoint = (screenPosition + screenCenter) / 2;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(midPoint.x, midPoint.y, screenPosition.z)); 


        board.position = midPoint;
    }

    Action onHideAction;
    public void OkButton()
    {
        onOkButton?.Invoke();
        onHideAction?.Invoke();

        if (BuildingManager.Instance.NeedTutorial)
        {
            BuildingManager.Instance.nextTutorial = true;
        }
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

    public override void Show()
    {
        base.Show();
        firstLoad = false;
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

        if(firstLoad) Hide();
    }
}

