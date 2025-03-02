using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Rotater : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //public Slider xSlider;
    //public Slider ySlider;
    ////public Slider zSlider;
    //public Transform rotateTarget;

    //private void Awake()
    //{
    //    var euler = rotateTarget.rotation.eulerAngles;
    //    xSlider.SetValueWithoutNotify(euler.x);
    //    ySlider.SetValueWithoutNotify(euler.y);
    //    //zSlider.SetValueWithoutNotify(euler.z);
    //    xSlider.onValueChanged.AddListener(RotateTarget);
    //    ySlider.onValueChanged.AddListener(RotateTarget);
    //    //zSlider.onValueChanged.AddListener(RotateTarget);
    //}

    //private void OnDestroy()
    //{
    //    xSlider.onValueChanged.RemoveListener(RotateTarget);
    //    ySlider.onValueChanged.RemoveListener(RotateTarget);
    //    //zSlider.onValueChanged.RemoveListener(RotateTarget);
    //}

    //void RotateTarget(float val)
    //{
    //    rotateTarget.rotation = Quaternion.Euler(xSlider.value, ySlider.value, 0);
    //}

    private bool update = false;
    public Transform rotateTarget;
    public float horizontalSpeed = 1;
    public float verticalSpeed = 1;

    private void Update()
    {
        if (update && Input.GetMouseButton(0))
        {
            float h = -horizontalSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
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
