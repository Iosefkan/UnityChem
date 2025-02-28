using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotater : MonoBehaviour
{
    public Slider xSlider;
    public Slider ySlider;
    //public Slider zSlider;
    public Transform rotateTarget;

    private void Awake()
    {
        var euler = rotateTarget.rotation.eulerAngles;
        xSlider.SetValueWithoutNotify(euler.x);
        ySlider.SetValueWithoutNotify(euler.y);
        //zSlider.SetValueWithoutNotify(euler.z);
        xSlider.onValueChanged.AddListener(RotateTarget);
        ySlider.onValueChanged.AddListener(RotateTarget);
        //zSlider.onValueChanged.AddListener(RotateTarget);
    }

    private void OnDestroy()
    {
        xSlider.onValueChanged.RemoveListener(RotateTarget);
        ySlider.onValueChanged.RemoveListener(RotateTarget);
        //zSlider.onValueChanged.RemoveListener(RotateTarget);
    }

    void RotateTarget(float val)
    {
        rotateTarget.rotation = Quaternion.Euler(xSlider.value, ySlider.value, 0);
    }
}
