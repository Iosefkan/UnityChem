using TMPro;

public class IntValue : Value
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
            if (!int.TryParse(inputField.text, out int v))
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

    public override object DefaultVal
    {
        get { return 0; }
    }
}
