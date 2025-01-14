using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetCieLabText : MonoBehaviour
{
    public void SetColor(Color color)
    {
        var text = gameObject.GetComponent<TMP_Text>();
        var labColor = ColorHelper.RGBToLab(color);
        text.text = $"L = {labColor.x:f2}, a = {labColor.y:f2}, b = {labColor.z:f2}";
    }
}
