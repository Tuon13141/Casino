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

    [SerializeField] Text levelText;
    [SerializeField] Text moneyText;

    [SerializeField] Image levelSlider;

    [SerializeField] GameObject levelObject;
    [SerializeField] GameObject moneyObject;

    private void Start()
    {
        UserData userData = GameManager.Instance.UserData;
        moneyText.text = userData.money.ToString();
        float progress = (userData.currentExp - userData.startExp) / (userData.nextExp - userData.startExp);
        levelText.text = "Lv" + userData.level.ToString() + " : " + userData.currentExp.ToString("N0") + "/" + userData.nextExp.ToString("N0");

        levelSlider.fillAmount = progress;
    }
    public void SetCoinText(float money)
    {
        moneyText.text = money.ToString("N0");

        PlayScaleAnimation(moneyObject);
    }

    public void SetLevelProgress(float currentExp, float totalExp, float startExp, int level)
    {
        float progress = (currentExp - startExp) / (totalExp - startExp);
        levelText.text = "Lv" + level.ToString() + " : " + currentExp.ToString("N0") + "/" + totalExp.ToString("N0");

        levelSlider.fillAmount = progress;

        PlayScaleAnimation(moneyObject);
    }
    public void PlayScaleAnimation(GameObject gameObject)
    {
        animationContext.SetAnimationStrategy(new ScaleAnimation(new Vector3(1.2f, 1.2f, 1.2f), 0.5f));
        animationContext.PlayAnimation(gameObject);
    }
}
