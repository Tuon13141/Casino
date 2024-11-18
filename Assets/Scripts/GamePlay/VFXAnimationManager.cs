using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAnimationManager : Singleton<VFXAnimationManager>
{
    public void PlayPulseEffect(GameObject target, Vector3 minScale, Vector3 maxScale, float scaleSpeedTime, float time)
    {
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

        // Kết thúc hiệu ứng, trả về minScale
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

        // Đảm bảo scale chính xác tại thời điểm kết thúc
        targetTransform.localScale = endScale;
    }
}
