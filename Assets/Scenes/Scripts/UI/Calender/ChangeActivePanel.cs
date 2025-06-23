using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ChangeActivePanel : MonoBehaviour
{
    public Button button;
    //public GameObject disablePanel;
    //public GameObject enablePanel;
    public ScenarioManager scenarioManager;
    public Orchestrator orchestrator;
    public GameObject loadingPanel;
    public GameObject UI;
    public StartStopTrain extrTrain;

    private void Awake()
    {
        button.onClick.AddListener(ChangeActive);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(ChangeActive);
    }

    async void ChangeActive()
    {
        extrTrain.SetEnabled(false);
        loadingPanel.SetActive(true);
        await Task.Delay(50);
        var id = scenarioManager.GetScenarioId();
        if (id is null) return;
        //enablePanel.SetActive(true);
        //disablePanel.SetActive(false);
        orchestrator.Init(id.Value);
        loadingPanel.SetActive(false);
        UI.SetActive(false);
    }
}
