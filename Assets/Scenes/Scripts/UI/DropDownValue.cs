using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDownValue : Value
{
    private TMP_Dropdown dd;

    private void Start()
    {
        dd = GetComponentInChildren<TMP_Dropdown>();
    }

    public override double Val
    {
        get
        {
            return dd.value + 1;
        }
        set
        {
            dd.value = (int)value - 1;
        }
    }
}
