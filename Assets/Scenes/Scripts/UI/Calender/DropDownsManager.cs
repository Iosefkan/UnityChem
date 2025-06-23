using CalenderDatabase;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DropDownsManager : MonoBehaviour
{
    public TMP_Dropdown filmClusterDropdown;
    public TMP_Dropdown rollSettingsDropdown;
    public TMP_Dropdown scenarioDropDown;

    private void Awake()
    {
        filmClusterDropdown.ClearOptions();
        rollSettingsDropdown.ClearOptions();

        using var ctx = new CalenderContext();

        var clusterNames = ctx.FilmProfileClusters.Select(fpc => fpc.Name).ToList();
        filmClusterDropdown.AddOptions(clusterNames);

        var rollSettingsNames = ctx.RollSettings.Select(rs => rs.Name).ToList();
        rollSettingsDropdown.AddOptions(rollSettingsNames);

        scenarioDropDown.onValueChanged.AddListener(LoadData);
    }

    private void LoadData(int i)
    {
        using var ctx = new CalenderContext();

        var scenario = ctx.Scenarios.Include(s => s.RollSettings).Include(s => s.FilmProfileCluster)
            .Select(s => new ScenarioData()
            {
                ScenarioName = s.Name,
                ClusterName = s.FilmProfileCluster.Name,
                RollsName = s.RollSettings.Name
            })
            .FirstOrDefault(s => s.ScenarioName == scenarioDropDown.captionText.text);

        Debug.Log(scenario.ScenarioName);

        filmClusterDropdown.value = filmClusterDropdown.options.FindIndex(f => f.text == scenario.ClusterName);
        rollSettingsDropdown.value = rollSettingsDropdown.options.FindIndex(f => f.text == scenario.RollsName);
    }


    private class ScenarioData
    {
        public string ScenarioName { get; set; }
        public string ClusterName { get; set; }
        public string RollsName { get; set; }
    }
}
