using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static TabsManager;

public class TabsMore : MonoBehaviour
{
    [Serializable]
    struct Switch
    {
        public Button btn;
        public GameObject panel;
        public Vector2 pos;
    }
    [SerializeField] Switch[] switchs;

    Switch currSwt;
    Color prevBtnColor;

    void Awake()
    {
        foreach (Switch sw in switchs)
        {
            //Необходимо активировать, чтобы вызвались скрипты скрытых панелей
            sw.panel.SetActive(true);

            sw.btn.onClick.AddListener(() => ActivePanel(sw));

            RectTransform rectTr = sw.panel.GetComponent<RectTransform>();
            sw.pos.Set(0, rectTr.rect.height);

            HideTab(sw.panel);
        }
            
        if (switchs.Length > 0) ActivePanel(switchs.First());
    }

    void ActivePanel(Switch sw)
    {
        if (currSwt.panel) HideTab(currSwt.panel);
        if (currSwt.btn) currSwt.btn.GetComponent<Image>().color = prevBtnColor;

        currSwt = sw;
        Image btnImage = currSwt.btn.GetComponent<Image>();
        prevBtnColor = btnImage.color;
        btnImage.color = currSwt.panel.GetComponent<Image>().color;

        ShowTab(currSwt);
    }

    void HideTab(GameObject tab)
    {
        RectTransform rectTr = tab.GetComponent<RectTransform>();
        rectTr.anchoredPosition = new Vector2(0, -rectTr.rect.height*2);
    }

    void ShowTab(Switch sw)
    {
        RectTransform rectTr = sw.panel.GetComponent<RectTransform>();
        rectTr.anchoredPosition = sw.pos;
    }
}
