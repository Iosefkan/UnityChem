using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanelsDropDown : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    private GameObject currPanel;
    private int currIndex;
    private bool isInit = false;

    private void OnEnable()
    {
        if (isInit) return;
        isInit = true;

        foreach (GameObject panel in panels)
        {
            panel.SetActive(true);
            panel.GetComponent<Image>().enabled = true;
            if (panel.GetComponent<ContentSizeFitter>() == null)
            {
                var csf = panel.AddComponent<ContentSizeFitter>();
                csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            }
            if (panel.GetComponent<Mask>() == null)
            {
                panel.AddComponent<Mask>();
            }

            HidePanel(panel);
        }

        Switch(0);
    }

    public void Switch(int index)
    {
        if (currPanel != null)
        {
            HidePanel(currPanel);
        }

        currPanel = panels[index];
        ShowPanel(currPanel);
        currIndex = index;
    }

    public int CurrIndex()
    {
        return currIndex;
    }

    void HidePanel(GameObject panel)
    {
        panel.GetComponent<ContentSizeFitter>().enabled = false;
        panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
    }

    void ShowPanel(GameObject panel)
    {
        panel.GetComponent<ContentSizeFitter>().enabled = true;
    }
}
