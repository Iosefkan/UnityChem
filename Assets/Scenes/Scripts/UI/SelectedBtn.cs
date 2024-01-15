using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedBtn : MonoBehaviour
{
    public string Name;

    private bool isSelected = false;

    public string select(bool isSelect)
    {
        isSelected = isSelect;
        transform.GetChild(0).gameObject.SetActive(!isSelected);

        return Name;
    }
}
