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
        public MouseButton mouseBtn;
        public GameObject panel;
        public String text;
    }

    [SerializeField] TMP_Text header = null;
    [SerializeField] Switch[] switchs;

    Switch currSwt;

    void Awake()
    {
        foreach (Switch sw in switchs)
        {
            sw.btn.down.AddListener(() => ActivePanel(sw));
            sw.mouseBtn.click.AddListener(() => ActivePanel(sw));
            //���������� ������������, ����� ��������� ������� ������� �������
            sw.panel.SetActive(true);
            sw.panel.SetActive(false);
        }

        if (switchs.Length > 0) ActivePanel(switchs.First());
    }

    void ActivePanel(Switch sw)
    {
        if (currSwt.panel) currSwt.panel.SetActive(false);
        if (currSwt.btn) currSwt.btn.GetComponent<SelectedBtn>().select(false);
        if (currSwt.mouseBtn) currSwt.mouseBtn.GetComponent<SelectedBtn>().select(false);

        currSwt = sw;

        currSwt.panel.SetActive(true);
        currSwt.btn.GetComponent<SelectedBtn>().select(true);
        currSwt.mouseBtn.GetComponent<SelectedBtn>().select(true);
        if (header != null) header.text = currSwt.text;
    }
}