using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanels : MonoBehaviour
{
    [Serializable]
    struct Switch
    {
        public VrButton btn;
        public GameObject panel;
        public String text;
    }

    [SerializeField] TMP_Text header = null;
    [SerializeField] Switch[] switchs;

    Switch currSwt;

    void Start()
    {
        foreach (Switch sw in switchs)
        {
            sw.btn.down.AddListener(() => ActivePanel(sw));
            //Необходимо активировать, чтобы вызвались скрипты скрытых панелей
            sw.panel.SetActive(true);
            sw.panel.SetActive(false);
        }

        if (switchs.Length > 0) ActivePanel(switchs.First());
    }

    void ActivePanel(Switch sw)
    {
        if (currSwt.panel) currSwt.panel.SetActive(false);
        if (currSwt.btn) currSwt.btn.GetComponent<SelectedBtn>().select(false);

        currSwt = sw;

        currSwt.panel.SetActive(true);
        currSwt.btn.GetComponent<SelectedBtn>().select(true);
        if (header != null) header.text = currSwt.text;
    }
}