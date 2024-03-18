using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanelsDropDown : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    private GameObject currPanel;

    private void Start()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        Switch(0);
    }

    public void Switch(int index)
    {
        if (currPanel != null)
        {
            currPanel.SetActive(false);
        }

        currPanel = panels[index];
        currPanel.SetActive(true);
    }
}
