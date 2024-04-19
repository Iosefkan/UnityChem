using TMPro;

public class DoubleValue : Value
{
    private TMP_InputField inputField;
    
    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        Val = 0;
    }

    public override object Val
    {
        get
        {
            if (!double.TryParse(inputField.text, out double v))
            {
                v = 0;
            }
            return v;
        }
        set
        {
            inputField.text = value.ToString();
        }
    }
}
