using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBox : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Vector3 worldScale = Vector3.one;
    public void SetUp(Transform targetTransform)
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            // 2. Đặt vị trí của UI object vào vị trí của transform truyền vào
            transform.parent = targetTransform;
            
            Vector3 vector3 = new Vector3(targetTransform.position.x, targetTransform.position.y + 10, targetTransform.position.z);
            transform.position = vector3;

             // 3. Đặt scale của UI object theo worldScale đã chỉ định
             transform.localScale = worldScale;
        }
        else
        {
            Debug.LogError("Canvas không phải là World Space Canvas");
        }
    }
    public void SetProgress(float progress, float maxProgress)
    {
        float fill = progress / maxProgress;
        this.fill.fillAmount = fill;
    }
}
