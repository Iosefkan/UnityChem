using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrTimeUpdate : MonoBehaviour
{
    TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = System.DateTime.Now.ToString("HH:mm:ss");
    }
}
