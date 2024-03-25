using UnityEngine;
using TMPro;

public class DropDownValue : Value
{
    private DropDownControl ddc;
    
    private void OnEnable()
    {
        init();
    }

    public override double Val
    {
        get
        {
            return ddc.CurrVal() + 1;
        }
        set
        {
            ddc.SetVal((int)value - 1);
        }
    }

    private void init()
    {
        if (ddc == null)
            ddc = GetComponentInChildren<DropDownControl>();
    }
}
