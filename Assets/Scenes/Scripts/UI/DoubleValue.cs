using TMPro;
using UnityEngine;

public class DoubleValue : Value
{
    [SerializeField] private double defaultValue = 0;

    private TMP_InputField inputField;
    
    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
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
}
