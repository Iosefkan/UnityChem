using TMPro;

public class StringValue : Value
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        if (inputField == null)
        {
            inputField = GetComponent<TMP_InputField>();
        }
        Val = string.Empty;
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
}
