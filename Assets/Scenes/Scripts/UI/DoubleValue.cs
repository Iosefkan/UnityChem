using UnityEngine;
using TMPro;

public class DoubleValue : Value
{
    private TMP_InputField inputField;
    
    private void OnEnable()
    {
        init();
    }

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

    private void init()
    {
        if (inputField == null)
        {
            inputField = GetComponentInChildren<TMP_InputField>();
            Val = 0;
        }
    }
}
