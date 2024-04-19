using TMPro;
using UnityEngine;

public class DropDownControl : MonoBehaviour
{
    private SwitchPanelsDropDown sdd;
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        sdd = GetComponent<SwitchPanelsDropDown>();
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void SetVal(int i)
    {
        dropdown.value = i;
        if (sdd != null)
        {
            sdd.Switch(i);
        }
    }

    public int CurrVal()
    {
        return dropdown.value;      
    }
}
