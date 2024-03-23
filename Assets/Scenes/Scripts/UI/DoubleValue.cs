using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoubleValue : Value
{
    [SerializeField] private TMP_InputField inputField;

    public override double Val
    {
        get
        {
            return double.Parse(inputField.text);
        }
        set
        {
            inputField.text = value.ToString();
        }
    }


    public void Print()
    {
        Debug.Log(Val);
    }
}
