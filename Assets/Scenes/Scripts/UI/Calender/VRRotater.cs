using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRRotater : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    private bool update = false;
    public Transform rotateTarget;
    public float horizontalSpeed = 1;
    public float scrollMult = 1;

    public void OnDrag(PointerEventData eventData)
    {
        if (update)
        {
            float h = -horizontalSpeed * eventData.delta.x * scrollMult * Time.deltaTime;
            rotateTarget.Rotate(0, h, 0);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        update = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        update = false;
    }
}
