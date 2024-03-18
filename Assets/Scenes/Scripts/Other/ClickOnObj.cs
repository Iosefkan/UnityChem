using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class ClickOnObj : MonoBehaviour
{
    readonly int leftMouseBtn = (int)PointerEventData.InputButton.Left;
    readonly int rightMouseBtn = (int)PointerEventData.InputButton.Right;

    [SerializeField] private int rangePressing = 10;

    private void Update()
    {
        if (Input.GetMouseButtonDown(leftMouseBtn) || Input.GetMouseButtonDown(rightMouseBtn))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, rangePressing))
            {
                ClickedObj clObj = hit.collider.gameObject.GetComponent<ClickedObj>();
                if (clObj != null)
                {
                    if (Input.GetMouseButtonDown(leftMouseBtn))
                    {
                        clObj.Click(PointerEventData.InputButton.Left);
                    }
                    else if (Input.GetMouseButtonDown(rightMouseBtn))
                    {
                        clObj.Click(PointerEventData.InputButton.Right);
                    }
                }
            }
        }
    }
}
