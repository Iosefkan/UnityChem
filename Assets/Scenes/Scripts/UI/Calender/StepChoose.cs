using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepChoose : MonoBehaviour
{
    public Button extruderButton;
    public GameObject extruderPanel;
    public Button calenderButton;
    public GameObject calenderPanel;

    void Start()
    {
        extruderButton.onClick.AddListener(() => SetPanelActive(extruderPanel));
        calenderButton.onClick.AddListener(() => SetPanelActive(calenderPanel));
    }

    private void OnDestroy()
    {
        extruderButton.onClick.RemoveAllListeners();
        calenderButton.onClick.RemoveAllListeners();
    }

    void SetPanelActive(GameObject go)
    {
        go.SetActive(true);
        gameObject.SetActive(false);
    }
}
