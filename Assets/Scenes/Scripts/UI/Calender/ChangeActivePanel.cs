using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeActivePanel : MonoBehaviour
{
    public Button button;
    public GameObject disablePanel;
    public GameObject enablePanel;
    public ScenarioManager scenarioManager;
    public Orchestrator orchestrator;

    private void Awake()
    {
        button.onClick.AddListener(ChangeActive);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(ChangeActive);
    }

    void ChangeActive()
    {
        var id = scenarioManager.GetScenarioId();
        if (id is null) return;
        enablePanel.SetActive(true);
        disablePanel.SetActive(false);
        orchestrator.Init(id.Value);
    }
}
