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
            int val = (int)value - 1;

            ddc.SetVal(val >= 0 ? val : 0);
        }
    }
}
