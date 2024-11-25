using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorial : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => true;

    public override bool UseBehindPanel => false;

    [SerializeField] GameObject stage1;
    [SerializeField] GameObject stage2;

    public void PlayStageOne()
    {
        stage1.SetActive(true);
        stage2.SetActive(false);
    }

    public void PlayStageTwo()
    {
        stage1.SetActive(false);
        stage2.SetActive(true);
    }
}
