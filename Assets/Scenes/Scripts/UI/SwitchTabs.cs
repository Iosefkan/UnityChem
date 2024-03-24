using System;
using System.Linq;
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

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] Switch[] switchs;

    Switch currSwt;
    Color prevBtnColor;

    void Awake()
    {
        foreach (Switch sw in switchs)
        {
            sw.btn.onClick.AddListener(() => ActivePanel(sw));
            //Необходимо активировать, чтобы вызвались скрипты скрытых панелей
            HideTab(sw.panel);
            sw.panel.SetActive(true);
        }

        if (switchs.Length > 0) ActivePanel(switchs.First());
    }

    void ActivePanel(Switch sw)
    {
        if (currSwt.panel) HideTab(currSwt.panel);
        if (currSwt.btn) currSwt.btn.GetComponent<Image>().color = prevBtnColor;

        currSwt = sw;
        scrollRect.content = currSwt.panel.GetComponent<RectTransform>();
        //currSwt.panel.SetActive(true);
        Image btnImage = currSwt.btn.GetComponent<Image>();
        prevBtnColor = btnImage.color;
        btnImage.color = currSwt.panel.GetComponent<Image>().color;
    }

    void HideTab(GameObject tab)
    {
        RectTransform rectTr = tab.GetComponent<RectTransform>();
        rectTr.anchoredPosition = new UnityEngine.Vector2(0, -rectTr.rect.height);
    }
}