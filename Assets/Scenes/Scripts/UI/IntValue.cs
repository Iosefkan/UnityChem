using TMPro;

public class IntValue : Value
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
            return int.Parse(inputField.text);
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
