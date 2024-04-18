using TMPro;

public class StringValue : Value
{
    private TMP_InputField inputField;

    private void OnEnable()
    {
        init();
    }

    public override object Val
    {
        get
        {
            return inputField.text;
        }
        set
        {
            inputField.text = (string)value;
        }
    }

    private void init()
    {
        if (inputField == null)
        {
            inputField = GetComponentInChildren<TMP_InputField>();
            Val = string.Empty;
        }
    }
}
