using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanelsDropDown : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    private GameObject currPanel;
    public  int currIndex;

    private void Awake()
    {
        foreach (GameObject panel in panels)
        {
            if (panel == null) continue;

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
        if (currPanel != null) HidePanel(currPanel); 

        currPanel = panels[index];
        currIndex = index;
        if (currPanel != null) ShowPanel(currPanel);
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

        VerticalLayoutGroup[] pl = panel.GetComponentsInParent<VerticalLayoutGroup>();
        foreach(var p in  pl)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(p.transform.GetComponent<RectTransform>());
        }
    }
}
