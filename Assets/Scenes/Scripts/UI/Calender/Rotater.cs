using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Rotater : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool update = false;
    public Transform rotateTarget;
    public float horizontalSpeed = 1;
    public float scrollMult = 1;

    private void Update()
    {
        if (update && Input.GetMouseButton(0))
        {
            float h = -horizontalSpeed * Input.GetAxis("Mouse X") * scrollMult * Time.deltaTime;
            // float v = verticalSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
            rotateTarget.Rotate(0, h, 0);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        update = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        update = false;
    }
}
