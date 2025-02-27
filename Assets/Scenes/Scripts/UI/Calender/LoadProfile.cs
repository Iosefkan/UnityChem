using CalenderDatabase;
using SimpleFileBrowser;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadProfile : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private WindowGraph graph;
    [SerializeField] private ProfileClusterManager profileManager;

    public void Start()
    {
        button.onClick.AddListener(Browse);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(Browse);
    }

    public void Browse()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Таблица", ".csv", ".txt"));
        FileBrowser.ShowLoadDialog(OnProfileSelected, null, FileBrowser.PickMode.Files, title: "Выберите файл с профилем");
    }

    public void OnProfileSelected(string[] paths)
    {
        var profiles = new List<FilmProfile>();
        graph.Clear();
        Debug.Log("Selected: " + paths[0]);
        string[] lines = File.ReadAllLines(paths[0]);
        string[][] values = lines.Select(line => line.Split(',')).ToArray();
        int profileCount = values.Max(val => val.Length) - 1;
        for (int i = 0; i < profileCount; i++)
        {
            var profile = new FilmProfile() { Id = i + 1 };
            profile.Profile = new();
            profiles.Add(profile);

        }

        Debug.Log("Profile count " + profiles.Count);
        Debug.Log("values " + values.Length + "  " + values[0].Length);

        for (int l = 0; l < values.Length; l++)
        {
            if (!double.TryParse(values[l][0], out var coordinate))
            {
                continue;
            }

            for (int i = 1; i < values[l].Length; i++)
            {
                if (!double.TryParse(values[l][i], out var thickness))
                {
                    continue;
                }



                profiles[i - 1].Profile.Add(new ProfilePoint() { Id = l + 1, Thickness = thickness, WidthPoint = coordinate });

            }
        }
        profileManager.SetProfiles(profiles);
    }
}
