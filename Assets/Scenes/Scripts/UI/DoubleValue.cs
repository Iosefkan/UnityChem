using System;
using TMPro;
using UnityEngine;

public class DoubleValue : Value
{
    [SerializeField] private double defaultValue = 0;
    //[SerializeField] private bool useMinMax = false;
    //[SerializeField] private double minValue = 0;
    //[SerializeField] private double maxValue = 0;

    private TMP_InputField inputField;
    
    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        //var validator = ScriptableObject.CreateInstance<TMP_DoubleValidator>();
        //validator.Init(useMinMax, minValue, maxValue);
        //inputField.inputValidator = validator;
        Val = defaultValue;
    }

    public override object Val
    {
        get
        {
            if (!double.TryParse(inputField.text, out double v))
            {
                v = defaultValue;
            }
            return v;
        }
        set
        {
            inputField.text = value.ToString();
        }
    }

    public override object DefaultVal
    {
        get { return defaultValue; }
    }

    //public class TMP_DoubleValidator : TMP_InputValidator
    //{
    //    private double minValue;
    //    private double maxValue;
    //    private bool useMinMax;

    //    public void Init(bool use, double minValue, double maxValue)
    //    {
    //        this.minValue = minValue;
    //        this.maxValue = maxValue;
    //        useMinMax = use;
    //    }

    //    public override char Validate(ref string text, ref int pos, char ch)
    //    {
    //        if (double.TryParse(text + ch, out var res))
    //        {
    //            if (useMinMax)
    //            {
    //                if (res >= minValue && res <= maxValue)
    //                {
    //                    text += ch;
    //                    pos += 1;
    //                    return ch;
    //                }
    //                else return (char)0;
    //            }
    //            else
    //            {
    //                text += ch;
    //                pos += 1;
    //                return ch;
    //            }

    //        }
    //        return (char)0;
    //    }
    //}
}
