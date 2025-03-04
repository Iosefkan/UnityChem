using CalenderDatabase;
using Microsoft.EntityFrameworkCore;
using Program;
using System;
using System.Linq;
using System.Threading.Tasks;
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

    public IntValue averagedCount;
    public IntValue tableSkipStep;
    public DoubleValue averagedWeight;
    public DoubleValue lastWeight;



    private long? currentId;

    private async void Awake()
    {
        scenarioDrop.onValueChanged.AddListener(_ => LoadData());
        UpdateOptions();

        addScenario.onClick.AddListener(AddData);
        saveScenario.onClick.AddListener(SaveData);

        await InitData().ConfigureAwait(false);
    }

    async Task InitData()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        if (scenarioDrop.options.Count > 0) {
            Debug.Log("Init");
            scenarioDrop.onValueChanged.Invoke(0);
        }
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
        else if (scenarioDrop.options.Count > 0 && !string.IsNullOrEmpty(currentText))
        {
            scenarioDrop.onValueChanged.Invoke(0);
        }
        //else scenarioDrop.onValueChanged.Invoke(0);
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

        averagedCount.Val = scenario.AveragedProfilesCount;
        averagedWeight.Val = scenario.AveragedProfileWeight;
        lastWeight.Val = scenario.LastProfileWeight;
        tableSkipStep.Val = scenario.TableSkipStep;
    }

    void ClearScenario()
    {
        currentId = null;
        scenarioName.Val = "";

        thicknessMax.Val = 0d;
        time.Val = 0;
        tableSkipStep.Val = 4;

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
        var scenario = new Scenario() { Name = name, RollSettingsId = roll.Id, FilmProfileClusterId = profile.Id, TableSkipStep = 4 };
        ctx.Scenarios.Add(scenario);
        ctx.SaveChanges();
        SetScenario(scenario);
        UpdateOptions();
        int index = scenarioDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == scenario.Name);
        if (index > -1) scenarioDrop.SetValueWithoutNotify(index);
    }

    void SaveData()
    {
        Debug.Log("Saving scenario");
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
        scenario.TableSkipStep = (int)tableSkipStep.Val;
        scenario.IsRange = rangeTask.isOn;
        scenario.ThicknessMax = (double)thicknessMax.Val;

        scenario.AveragedProfilesCount = (int)averagedCount.Val;
        scenario.AveragedProfileWeight = (double)averagedWeight.Val;
        scenario.LastProfileWeight = (double)lastWeight.Val;

        ctx.SaveChanges();
        Debug.Log("Scenario saved");
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
