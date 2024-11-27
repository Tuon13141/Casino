using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIElement
{
    private ObjectAnimationContext animationContext = new ObjectAnimationContext();
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => false;

    [SerializeField] Text expText;
    [SerializeField] Text levelText;
    [SerializeField] Text moneyText;

    [SerializeField] Image levelSlider;

    [SerializeField] GameObject levelObject;
    [SerializeField] GameObject moneyObject;

    [SerializeField] Button updateButton;
    private void Start()
    {
        UserData userData = GameManager.Instance.UserData;
        moneyText.text = userData.money.ToString();
        float progress = (userData.currentExp - userData.startExp) / (userData.nextExp - userData.startExp);
        expText.text = userData.currentExp.ToString("N0") + "/" + userData.nextExp.ToString("N0");
        levelText.text = "Lv" + userData.level.ToString();
        levelSlider.fillAmount = progress;

        updateButton.onClick.AddListener(UpdateButton);
    }
    public void SetCoinText(float money)
    {
        moneyText.text = money.ToString("N0");

        PlayScaleAnimation(moneyObject);
    }

    public void SetLevelProgress(float currentExp, float totalExp, float startExp, int level)
    {
        float progress = (currentExp - startExp) / (totalExp - startExp);
        expText.text =currentExp.ToString("N0") + "/" + totalExp.ToString("N0");
        levelText.text = "Lv" + level.ToString();
        levelSlider.fillAmount = progress;

        PlayScaleAnimation(moneyObject);
    }
    public void PlayScaleAnimation(GameObject gameObject)
    {
        animationContext.SetAnimationStrategy(new ScaleAnimation(new Vector3(1.1f, 1.1f, 1.1f), 0.5f));
        animationContext.PlayAnimation(gameObject);
    }

    public void UpdateButton()
    {
        GameUI.Instance.Get<UIUpgrade>().Show();
    }

    public void ShowUpdateButton(bool b)
    {
        updateButton.SetActive(b);
    }
}
