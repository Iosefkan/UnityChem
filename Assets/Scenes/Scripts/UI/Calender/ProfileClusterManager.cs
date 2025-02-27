using CalenderDatabase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileClusterManager : MonoBehaviour
{
    public StringValue profileClusterName;
    public TMP_Dropdown filmDrop;
    public TMP_Dropdown scenarioDrop;
    public TMP_Dropdown profileClusterDrop;
    public Button addProfile;
    public Button saveProfile;

    public DoubleValue width;
    public DoubleValue curveStart;
    public DoubleValue crossStart;
    public WindowGraph profileGraph;

    private List<FilmProfile> filmProfiles;
    private long? currentId;

    private void Awake()
    {
        profileClusterDrop.onValueChanged.AddListener(_ => LoadData(Source.Profile));
        filmDrop.onValueChanged.AddListener(_ => LoadData(Source.Film));
        scenarioDrop.onValueChanged.AddListener(_ => LoadData(Source.Scenario));
        UpdateOptions();
        addProfile.onClick.AddListener(AddData);
        saveProfile.onClick.AddListener(SaveData);
    }

    enum Source
    {
        Scenario,
        Profile,
        Film
    }

    void LoadData(Source source)
    {
        using var ctx = new CalenderContext();
        var profileCluster = source switch
        {
            Source.Scenario => ctx.Scenarios.Include(s => s.FilmProfileCluster).ThenInclude(pc => pc.FilmProfiles).AsNoTracking().FirstOrDefault(s => s.Name == scenarioDrop.captionText.text).FilmProfileCluster,
            Source.Film => ctx.FilmProfileClusters.Include(pc => pc.FilmProfiles).Include(pc => pc.Film).AsNoTracking().FirstOrDefault(f => f.Film.Name == filmDrop.captionText.text),
            Source.Profile => ctx.FilmProfileClusters.Include(pc => pc.FilmProfiles).AsNoTracking().FirstOrDefault(pc => pc.Name == profileClusterDrop.captionText.text)
        };
        ClearProfileCluster();
        SetProfileCluster(profileCluster);
        UpdateOptions();
    }

    void UpdateOptions()
    {
        var currentText = profileClusterDrop.captionText.text;
        if (filmDrop?.captionText?.text is null) return;
        Debug.Log("Current " + currentText);
        Debug.Log(filmDrop.captionText.text);
        using var ctx = new CalenderContext();
        var profileClusterNames = ctx.Films
            .Include(f => f.FilmProfileClusters)
            .FirstOrDefault(f => f.Name == filmDrop.captionText.text)
            .FilmProfileClusters.Select(pc => pc.Name).ToList();
        profileClusterDrop.ClearOptions();
        profileClusterDrop.AddOptions(profileClusterNames);
        Debug.Log(string.Join(", ", profileClusterNames));
        int index = profileClusterDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == currentText);
        if (index > -1) profileClusterDrop.SetValueWithoutNotify(index);
        profileClusterDrop.RefreshShownValue();
    }

    void SetProfileCluster(FilmProfileCluster profileCluster)
    {
        if (profileCluster is null) return;
        currentId = profileCluster.Id;
        SetProfiles(profileCluster.FilmProfiles?.ToList());
        width.Val = profileCluster.Width;
        profileClusterName.Val = profileCluster.Name;
        crossStart.Val = profileCluster.CrossStart;
        curveStart.Val = profileCluster.CurveStart;
    }

    void ClearProfileCluster()
    {
        currentId = null;
        filmProfiles = null;
        width.Val = 0d;
        crossStart.Val = 0d;
        curveStart.Val = 0d;
        profileGraph.Clear();
        profileClusterName.Val = "";
    }

    void AddData()
    {
        Debug.Log("Adding profile");
        using var ctx = new CalenderContext();
        Debug.Log("Adding cluster " + filmDrop.captionText.text);
        var film = ctx.Films.AsNoTracking().FirstOrDefault(f => f.Name == filmDrop.captionText.text);
        Debug.Log(film);
        if (film is null) return;
        var name = "Профиль " + DateTime.Now.ToString("hh:mm");
        var profileCluster = new FilmProfileCluster() { Name = name, FilmId = film.Id };
        ctx.FilmProfileClusters.Add(profileCluster);
        ctx.SaveChanges();
        SetProfileCluster(profileCluster);
        UpdateOptions();
        int index = profileClusterDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == profileCluster.Name);
        if (index > -1) profileClusterDrop.SetValueWithoutNotify(index);
    }

    void SaveData()
    {
        if (currentId is null) return;
        using var ctx = new CalenderContext();
        var profileCluster = ctx.FilmProfileClusters.Find(currentId);

        var profiles = ctx.FilmProfiles.Where(fp => fp.FilmProfileClusterId == profileCluster.Id);
        ctx.FilmProfiles.RemoveRange(profiles);

        profileCluster.Width = (double)width.Val;
        profileCluster.CurveStart = (double)curveStart.Val;
        profileCluster.CrossStart = (double)crossStart.Val;

        profileCluster.FilmProfiles = filmProfiles;
        profileCluster.Name = (string)profileClusterName.Val;
        ctx.SaveChanges();
        UpdateOptions();
    }

    void DeleteData(string name)
    {
        if (string.IsNullOrEmpty(name)) return;
        using var ctx = new CalenderContext();
        var profileCluster = ctx.FilmProfileClusters.First(c => c.Name == name);
        var profiles = ctx.FilmProfiles.Where(fp => fp.FilmProfileClusterId == profileCluster.Id);
        ctx.FilmProfiles.RemoveRange(profiles);
        if (currentId == profileCluster.Id) ClearProfileCluster();
        ctx.FilmProfileClusters.Remove(profileCluster);
        ctx.SaveChanges();
        UpdateOptions();
        profileClusterDrop.Refresh();
    }

    public void RemoveData(Button remBtn)
    {
        string optText = remBtn.transform.parent.GetComponentInChildren<TMP_Text>().text;
        DeleteData(optText);
    }

    public void SetProfiles(List<FilmProfile> profiles)
    {
        filmProfiles?.Clear();
        profileGraph.Clear();
        if (profiles is null || profiles.Count == 0 || currentId is null) return;
        foreach (var profile in profiles)
        {
            profile.FilmProfileClusterId = currentId.Value;
        }
        filmProfiles = profiles;
        var points = profiles.Last().Profile;
        Debug.Log(points.Count);
        foreach (ProfilePoint point in points)
        {
            profileGraph.AddData(new System.Numerics.Vector2((float)point.WidthPoint, (float)point.Thickness));
        }
    }
}