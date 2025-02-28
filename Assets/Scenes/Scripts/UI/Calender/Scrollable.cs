using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scrollable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform scrollResizeTarget;
    public float max;
    public float min;

    private float start;
    private bool update = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        update = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        update = false;
    }

    private void Awake()
    {
        start = scrollResizeTarget.localScale.z;
    }

    private void Update()
    {
        if (!update) return;
        var scale = scrollResizeTarget.localScale.z / start;
        var del = Input.mouseScrollDelta.y;
        if (scale > max && del > 0) return;
        if (scale < min && del < 0) return;
        if (del != 0)
        {
            var value = del > 0 ? 1.05f : 0.95f;
            scrollResizeTarget.localScale *= value;
        }
    }
}
