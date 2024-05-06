public class DropDownValue : Value
{
    private DropDownControl ddc;
    
    private void Awake()
    {
        ddc = GetComponentInChildren<DropDownControl>();
    }

    public override object Val
    {
        get
        {
            return ddc.CurrVal() + 1;
        }
        set
        {
            int val = 0;
            if (value is double)
                val = (int)(double)value;
            if (value is int)
                val = (int)value;
            if (value is long)
                val = (int)(long)value;

            val -= 1;
            ddc.SetVal(val >= 0 ? val : 0);
        }
    }

    public override object DefaultVal
    {
        get { return 1; }
    }
}
