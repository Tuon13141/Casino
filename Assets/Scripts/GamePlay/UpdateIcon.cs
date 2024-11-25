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
            GameUI.Instance.Get<UIOpenBuilding>().SetUp(buildingObject.GetBuildCost(), buildingObject.GetMoneyEarnedPerPassgenger(), buildingObject.Build, buildingObject.BuildingSO.icon);
        }
        else
        {
            GameUI.Instance.Get<UIUpdateBuilding>().Show();
            GameUI.Instance.Get<UIUpdateBuilding>().SetUp(buildingObject.GetNextUpdateCost(), buildingObject.GetNextMoneyEarnedPerPassgenger(), buildingObject.UpdateBuilding, buildingObject.level, buildingObject.transform);
        }   

       
    }

    public void PlayPulseEffect(Vector3 minScale, Vector3 maxScale, float scaleSpeedTime, float time)
    {
        GameObject target = this.gameObject;
        if (target == null)
        {
            Debug.LogError("Target GameObject is null.");
            return;
        }

        StartCoroutine(PulseEffectCoroutine(target, minScale, maxScale, scaleSpeedTime, time));
    }

    private IEnumerator PulseEffectCoroutine(GameObject target, Vector3 minScale, Vector3 maxScale, float scaleSpeedTime, float time)
    {
        Transform targetTransform = target.transform;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            if (target == null)
            {
                Debug.LogError("Target GameObject is null.");
                yield break;
            }
            yield return ScaleOverTime(targetTransform, minScale, maxScale, scaleSpeedTime);
            yield return ScaleOverTime(targetTransform, maxScale, minScale, scaleSpeedTime);

            elapsedTime += scaleSpeedTime * 2f;
        }

        targetTransform.localScale = minScale;
    }

    private IEnumerator ScaleOverTime(Transform targetTransform, Vector3 startScale, Vector3 endScale, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            targetTransform.localScale = Vector3.Lerp(startScale, endScale, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        targetTransform.localScale = endScale;
    }
}
