using CalenderDatabase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour
{
    public StringValue scenarioName;
    public TMP_Dropdown scenarioDrop;
    public Button addScenario;
    public Button saveScenario;

    public TMP_Dropdown profileDrop;
    public TMP_Dropdown rollDrop;

    public Toggle minTask;
    public Toggle rangeTask;
    public DoubleValue thicknessMax;

    public IntValue time;

    public DoubleValue crossMin;
    public DoubleValue crossMax;
    public DoubleValue crossDelta;

    public DoubleValue curveMin;
    public DoubleValue curveMax;
    public DoubleValue curveDelta;

    public IntValue averagedCount;
    public DoubleValue averagedWeight;
    public DoubleValue lastWeight;



    private long? currentId;

    private void Awake()
    {
        scenarioDrop.onValueChanged.AddListener(_ => LoadData());
        UpdateOptions();

        addScenario.onClick.AddListener(AddData);
        saveScenario.onClick.AddListener(SaveData);
        if (scenarioDrop.options.Count > 0) scenarioDrop.value = 0;
    }


    void LoadData()
    {
        using var ctx = new CalenderContext();
        var scenario = ctx.Scenarios.AsNoTracking().FirstOrDefault(s => s.Name == scenarioDrop.captionText.text);
        SetScenario(scenario);
        UpdateOptions();
    }

    void UpdateOptions()
    {
        var currentText = scenarioDrop.captionText.text;
        using var ctx = new CalenderContext();
        var scenarioNames = ctx.Scenarios.Select(s => s.Name).ToList();
        scenarioDrop.ClearOptions();
        scenarioDrop.AddOptions(scenarioNames);
        int index = scenarioDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == currentText);
        if (index > -1) scenarioDrop.SetValueWithoutNotify(index);
        scenarioDrop.RefreshShownValue();
    }

    void SetScenario(Scenario scenario)
    {
        if (scenario is null) return;
        currentId = scenario.Id;
        scenarioName.Val = scenario.Name;

        if (scenario.IsRange) rangeTask.isOn = true;
        else minTask.isOn = true;
        thicknessMax.Val = scenario.ThicknessMax ?? 0d;
        time.Val = scenario.Minutes;

        crossMin.Val = scenario.CrossMin;
        crossMax.Val = scenario.CrossMax;
        crossDelta.Val = scenario.CrossDelta;

        curveMin.Val = scenario.CurveMin;
        curveMax.Val = scenario.CurveMax;
        curveDelta.Val = scenario.CurveDelta;

        averagedCount.Val = scenario.AveragedProfilesCount;
        averagedWeight.Val = scenario.AveragedProfileWeight;
        lastWeight.Val = scenario.LastProfileWeight;
    }

    void ClearScenario()
    {
        currentId = null;
        scenarioName.Val = "";

        thicknessMax.Val = 0d;
        time.Val = 0;

        crossMin.Val = 0d;
        crossMax.Val = 0d;
        crossDelta.Val = 0d;

        curveMin.Val = 0d;
        curveMax.Val = 0d;
        curveDelta.Val = 0d;

        averagedCount.Val =  0;
        averagedWeight.Val = 0d;
        lastWeight.Val = 0d;
    }

    void AddData()
    {
        using var ctx = new CalenderContext();
        var profile = ctx.FilmProfileClusters.FirstOrDefault(p => p.Name == profileDrop.captionText.text);
        var roll = ctx.RollSettings.FirstOrDefault(p => p.Name == rollDrop.captionText.text);
        if (profile is null || roll is null)
        {
            Debug.Log("Scenario error");
            return;
        }
        var name = "Сценарий " + DateTime.Now.ToString("hh:mm");
        var scenario = new Scenario() { Name = name, RollSettingsId = roll.Id, FilmProfileClusterId = profile.Id };
        ctx.Scenarios.Add(scenario);
        ctx.SaveChanges();
        SetScenario(scenario);
        UpdateOptions();
        int index = scenarioDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == scenario.Name);
        if (index > -1) scenarioDrop.SetValueWithoutNotify(index);
    }

    void SaveData()
    {
        if (currentId is null) return;
        using var ctx = new CalenderContext();
        var scenario = ctx.Scenarios.Find(currentId);
        var profileCluster = ctx.FilmProfileClusters.FirstOrDefault(p => p.Name == profileDrop.captionText.text);
        var roll = ctx.RollSettings.FirstOrDefault(p => p.Name == rollDrop.captionText.text);
        if (profileCluster is null || roll is null) return;

        scenario.FilmProfileClusterId = profileCluster.Id;
        scenario.RollSettingsId = roll.Id;

        scenario.Name = (string)scenarioName.Val;
        scenario.Minutes = (int)time.Val;
        scenario.IsRange = rangeTask.isOn;
        scenario.ThicknessMax = (double)thicknessMax.Val;

        scenario.CrossMin = (double)crossMin.Val;
        scenario.CrossMax = (double)crossMax.Val;
        scenario.CrossDelta = (double)crossDelta.Val;

        scenario.CurveMin = (double)curveMin.Val;
        scenario.CurveMax = (double)curveMax.Val;
        scenario.CurveDelta = (double)curveDelta.Val;

        scenario.AveragedProfilesCount = (int)averagedCount.Val;
        scenario.AveragedProfileWeight = (double)averagedWeight.Val;
        scenario.LastProfileWeight = (double)lastWeight.Val;

        ctx.SaveChanges();
        UpdateOptions();
    }

    void DeleteData(string name)
    {
        if (string.IsNullOrEmpty(name)) return;
        using var ctx = new CalenderContext();
        var scenario = ctx.Scenarios.First(c => c.Name == name);
        if (currentId == scenario.Id) ClearScenario();
        ctx.Scenarios.Remove(scenario);
        ctx.SaveChanges();
        UpdateOptions();
        scenarioDrop.Refresh();
    }

    public void RemoveData(Button remBtn)
    {
        string optText = remBtn.transform.parent.GetComponentInChildren<TMP_Text>().text;
        DeleteData(optText);
    }

    public long? GetScenarioId()
    {
        return currentId;
    }
}
