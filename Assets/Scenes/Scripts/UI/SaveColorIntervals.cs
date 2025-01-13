using Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public static class TransformExtensions
{
    public static List<GameObject> FindObjectsWithTag(this Transform parent, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
            }
        }
        return taggedGameObjects;
    }
}

public class SaveColorIntervals : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject intervalsPanel;
    [SerializeField] private TMP_Dropdown filmDD;
    [SerializeField] private DoubleValue maxDelE;

    public void Start()
    {
        button.onClick.AddListener(Save);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(Save);
    }

    private void Save()
    {
        var rows = intervalsPanel.transform.FindObjectsWithTag("ColorRow");
        using ExtrusionContext ctx = new();
        var film = ctx.Films.First(f => f.Type == filmDD.captionText.text);
        film.MaxDelE = (double)maxDelE.Val;

        ctx.ColorIntervals.RemoveRange(ctx.ColorIntervals.Where(ci => ci.FilmId == film.Id));
        List<ColorInterval> intervals = new();
        foreach (var row in rows) 
        {
            var lab = ColorHelper.RGBToLab(row.GetComponentInChildren<GetColor>().Get());
            var values = row.GetComponentsInChildren<DoubleValue>();
            var toggle = row.GetComponentInChildren<Toggle>();
            intervals.Add(new ColorInterval() 
            {
                FilmId = film.Id,
                L = lab.x,
                a = lab.y,
                b = lab.z,
                MaxDelE = (float)(double)values.First(v => v.name == "max_del").Val,
                MinDelE = (float)(double)values.First(v => v.name == "min_del").Val,
                IsBaseColor = toggle.isOn,
            });
        }
        ctx.ColorIntervals.AddRange(intervals);
        ctx.SaveChanges();
    }
}
