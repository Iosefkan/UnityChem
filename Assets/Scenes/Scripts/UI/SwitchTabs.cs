using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchTabs : MonoBehaviour
{
    [Serializable]
    struct Switch
    {
        public Button btn;
        public GameObject panel;
    }

    [SerializeField] Switch[] switchs;

    Switch currSwt;
    Color prevBtnColor;

    void Awake()
    {
        foreach (Switch sw in switchs)
        {
            sw.btn.onClick.AddListener(() => ActivePanel(sw));
            //Необходимо активировать, чтобы вызвались скрипты скрытых панелей
            sw.panel.SetActive(true);
            sw.panel.SetActive(false);
        }

        if (switchs.Length > 0) ActivePanel(switchs.First());
    }

    void ActivePanel(Switch sw)
    {
        if (currSwt.panel) currSwt.panel.SetActive(false);
        if (currSwt.btn) currSwt.btn.GetComponent<Image>().color = prevBtnColor;

        currSwt = sw;

        currSwt.panel.SetActive(true);
        Image btnImage = currSwt.btn.GetComponent<Image>();
        prevBtnColor = btnImage.color;
        btnImage.color = currSwt.panel.GetComponent<Image>().color;
    }
}