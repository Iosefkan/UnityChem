using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class BaseSelected : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    private double stMin, stMax;
    private DoubleValue min, max;

    public void Start()
    {
        toggle.onValueChanged.AddListener(sel => Selected(sel));
    }

    public void Awake()
    {
        var values = gameObject.GetComponentsInChildren<DoubleValue>();
        max = values.First(v => v.name == "max_del");
        min = values.First(v => v.name == "min_del");
    }

    public void OnDestroy()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    public void Selected(bool selected)
    {
        max.gameObject.GetComponentInChildren<TMP_InputField>().interactable = !selected;
        min.gameObject.GetComponentInChildren<TMP_InputField>().interactable = !selected;
        if (selected) Sel();
        else Unsel();
    }

    private void Sel()
    {
        stMin = (double)min.Val;
        stMax = (double)max.Val;
        min.Val = 0;
        max.Val = 0;
    }

    private void Unsel()
    {
        min.Val = stMin; 
        max.Val = stMax;
    }
}
