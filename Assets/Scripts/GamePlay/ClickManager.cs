using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    void Start()
    {
        Game.Update.AddTask(OnUpdate);
    }

    void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                UpdateIcon updateIcon = hit.collider.GetComponent<UpdateIcon>();
                if (updateIcon != null)
                {
                    updateIcon.OnClick();
                }
            }
        }
    }
}
