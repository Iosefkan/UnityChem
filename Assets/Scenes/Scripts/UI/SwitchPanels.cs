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
        public Button btn;
        public GameObject panel;
        public String text;
    }

    [SerializeField] TMP_Text header;
    [SerializeField] Switch[] switchs;

    Switch currSwt;

    void Start()
    {
        foreach (Switch sw in switchs)
        {
            sw.btn.onClick.AddListener(() => ActivePanel(sw));
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
        header.text = currSwt.text;
    }
}