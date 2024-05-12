using TMPro;
using UnityEngine;

public class IntValue : Value
{
    [SerializeField] private int defaultValue = 0;

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
            if (!int.TryParse(inputField.text, out int v))
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
