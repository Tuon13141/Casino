using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommonObjectAnimation 
{
    public void PlayAnimation(GameObject gameObject);
}
public class ObjectAnimationContext
{
    private ICommonObjectAnimation animationStrategy;

    public void SetAnimationStrategy(ICommonObjectAnimation strategy)
    {
        animationStrategy = strategy;
    }

    public void PlayAnimation(GameObject target)
    {
        animationStrategy?.PlayAnimation(target);
    }
}


public class ScaleAnimation : ICommonObjectAnimation
{
    private Vector3 scaleFactor;
    private float duration;

    public ScaleAnimation(Vector3 scaleFactor, float duration)
    {
        this.scaleFactor = scaleFactor;
        this.duration = duration;
    }

    public void PlayAnimation(GameObject target)
    {
        if (target != null)
        {
            MonoBehaviour monoBehaviour = target.GetComponent<MonoBehaviour>();
            if (monoBehaviour == null)
            {
                return;
            }

            monoBehaviour.StartCoroutine(PlayScaleAnimation(target));
        }
    }

    private IEnumerator PlayScaleAnimation(GameObject target)
    {
        Transform targetTransform = target.transform;

        Vector3 originalScale = targetTransform.localScale;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            targetTransform.localScale = Vector3.Lerp(originalScale, scaleFactor, t);
            yield return null;
        }

        targetTransform.localScale = scaleFactor;


        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (duration / 2);
            targetTransform.localScale = Vector3.Lerp(scaleFactor, originalScale, t);
            yield return null;
        }

        targetTransform.localScale = originalScale;
    }
}
