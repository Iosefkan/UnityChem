using UnityEngine;
using TMPro;

public class DropDownValue : Value
{
    private DropDownControl ddc;
    
    private void OnEnable()
    {
        init();
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

    private void init()
    {
        if (ddc == null)
            ddc = GetComponentInChildren<DropDownControl>();
    }
}
