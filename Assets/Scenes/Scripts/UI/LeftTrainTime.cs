using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeftTrainTime : MonoBehaviour
{
    [SerializeField] string text = "Осталось времени: ";
    [SerializeField] int warningTime = 5;
    [SerializeField] CurrTimeUpdate operTime;

    TMP_Text tmp_text;

    void Start()
    {
        tmp_text = GetComponent<TMP_Text>();
    }

    void Update()
    {   
        if (operTime.GetLeftMin() > 0)
        {
            tmp_text.color = operTime.GetLeftMin() < warningTime ? Color.red : Color.black;
            tmp_text.text = text + operTime.GetLeftTimeStr();
        }
    }
}
