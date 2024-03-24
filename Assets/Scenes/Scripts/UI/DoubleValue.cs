using TMPro;

public class DoubleValue : Value
{
    private TMP_InputField inputField;

    private void Start()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        Val = 0;
    }

    public override double Val
    {
        get
        {
            if (inputField.text == string.Empty) return 0;
            return double.Parse(inputField.text);
        }
        set
        {
            inputField.text = value.ToString();
        }
    }
}
