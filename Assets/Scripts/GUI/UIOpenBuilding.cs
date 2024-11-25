using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOpenBuilding : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] Button okButton;
    [SerializeField] Button backButton;

    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI earnText;
    [SerializeField] Image img;

    Action onOkButton;
    public void SetUp(float cost, float earn, Action onOkButton, Sprite sprite)
    {
        costText.text = cost.ToString();
        earnText.text = earn.ToString();
        img.sprite = sprite;
       
        this.onOkButton = onOkButton;
    }

    Action onHideAction;
    public void OkButton()
    {
        onOkButton?.Invoke();

        BackButton();
    }

    public void BackButton() 
    { 
        onHideAction?.Invoke();
        Hide();
    }

    private void Start()
    {
        okButton.onClick.AddListener(OkButton);
        backButton.onClick.AddListener(BackButton);
    }
}
