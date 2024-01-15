using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnOffTablo : MonoBehaviour
{
    bool IsOn = false;
    
    void Start()
    {
        —hangeOnOff(false);
    }

    public void —hangeOnOff(bool isOn)
    {
        IsOn = isOn;
        TMP_Text t = transform.GetComponentInChildren<TMP_Text>();
        
        if (IsOn)
        {
            ColorUtility.TryParseHtmlString("#35763B", out Color color);
            t.text = "¬ À";
            t.color = color;
        }
        else
        {
            ColorUtility.TryParseHtmlString("#493C3C", out Color color);
            t.text = "¬€ À";
            t.color = color;
        }
    }

    public bool isOn()
    {
        return IsOn;
    }
}
