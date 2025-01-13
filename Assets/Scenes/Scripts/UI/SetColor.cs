using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetColor : MonoBehaviour
{
    public void Set(Color color)
    {
        gameObject.GetComponent<Image>().color = color;
    }
}
