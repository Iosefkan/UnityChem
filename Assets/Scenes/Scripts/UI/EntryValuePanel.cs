using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EntryValuePanel;

public class EntryValuePanel : MonoBehaviour
{
    public delegate void ChangeVal(TMP_Text message);
    public event ChangeVal? OnChangeVal;

    Regex inReg = new Regex("^(\\d?){0,3}(,(\\d?){0,2})?$");
    Regex outReg1 = new Regex("^.*,\\d\\d$");
    Regex outReg2 = new Regex("^\\d.*");

    [SerializeField] Button[] valBtns = new Button[10];
    [SerializeField] Button commaBtn;
    [SerializeField] Button delBtn;
    [SerializeField] Button entrBtn;

    [SerializeField] TMP_Text valText;

    TMP_Text targetField;

    void Start()
    {
        for (int i = 0; i < valBtns.Length; ++i)
        {
            int elem = i;
            valBtns[i].onClick.AddListener(() =>
            {
                string resStr = valText.text + elem.ToString();
                if (inReg.IsMatch(resStr))
                {
                    valText.text = resStr;
                }                
            });
        }

        commaBtn.onClick.AddListener(() =>
        {
            string resStr = valText.text + ",";
            if (inReg.IsMatch(resStr))
            {
                valText.text = resStr;
            }
        });

        delBtn.onClick.AddListener(() =>
        {
            if (valText.text.Length == 0) return;
            valText.text = valText.text.Substring(0, valText.text.Length - 1);
        });

        entrBtn.onClick.AddListener(() =>
        {
            //if (valText.text.Length == 0) valText.text = "0,00";
            //int breakCounter = 0;
            //while (!outReg1.IsMatch(valText.text))
            //{
            //    valText.text += "0";
            //    if (++breakCounter == 20)
            //    {
            //        break;
            //    }
            //}
            //if (!outReg2.IsMatch(valText.text)) valText.text = "0" + valText.text;

            targetField.text = string.Format("{0:f}", double.Parse(valText.text));
            OnChangeVal?.Invoke(targetField);
            gameObject.SetActive(false);
        });
    }

    public void Open(TMP_Text t)
    {
        targetField = t;
        valText.text = t.text;
        gameObject.SetActive(true);
    }
}
