using Database;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadColorIntervals : MonoBehaviour
{
    [SerializeField] private GameObject intervalsPanel;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private TMP_Dropdown filmDD;
    [SerializeField] private DoubleValue maxDelE;

    public void Awake()
    {
        filmDD.onValueChanged.AddListener((int _) => Load());
    }

    public void Load()
    {
        using var ctx = new ExtrusionContext();
        var film = ctx.Films.AsNoTracking().FirstOrDefault(f => f.Type == filmDD.captionText.text);
        var rows = intervalsPanel.transform.FindObjectsWithTag("ColorRow");
        foreach (var row in rows)
        {
            Destroy(row);
        }
        if (film is null) return;

        maxDelE.Val = film.MaxDelE;
        var intervals = ctx.ColorIntervals.Where(ci => ci.FilmId == film.Id).AsNoTracking().ToList();

        foreach (var interval in intervals)
        {
            var row = Instantiate(rowPrefab);
            row.transform.SetParent(intervalsPanel.transform, false);

            var values = row.GetComponentsInChildren<DoubleValue>();
            values.First(v => v.name == "max_del").Val = interval.MaxDelE;
            values.First(v => v.name == "min_del").Val = interval.MinDelE;
            var toggle = row.GetComponentInChildren<Toggle>();
            toggle.isOn = interval.IsBaseColor;
            if (interval.IsBaseColor) row.GetComponent<BaseSelected>().Selected(interval.IsBaseColor);
            toggle.group = intervalsPanel.GetComponent<ToggleGroup>();
            row.GetComponentInChildren<SetColor>().Set(ColorHelper.LabToRGB(new Vector3(interval.L, interval.a, interval.b)));
        }
    }
}
