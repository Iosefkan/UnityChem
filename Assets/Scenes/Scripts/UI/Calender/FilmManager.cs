using CalenderDatabase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilmManager : MonoBehaviour
{
    public StringValue filmName;
    public TMP_Dropdown filmDrop;
    public TMP_Dropdown scenarioDrop;
    public Button addFilm;
    public Button saveFilm;

    private long? currentId;

    private void Awake()
    {
        filmDrop.onValueChanged.AddListener(_ => LoadData(Source.Film));
        scenarioDrop.onValueChanged.AddListener(_ => LoadData(Source.Scenario));
        UpdateOptions();

        addFilm.onClick.AddListener(AddData);
        saveFilm.onClick.AddListener(SaveData);
    }

    enum Source
    {
        Scenario,
        Film
    }

    void LoadData(Source source)
    {
        using var ctx = new CalenderContext();
        var film = source switch
        {
            Source.Scenario => ctx.Scenarios.Include(fp => fp.FilmProfileCluster).ThenInclude(fp => fp.Film).AsNoTracking().FirstOrDefault(s => s.Name == scenarioDrop.captionText.text).FilmProfileCluster.Film,
            Source.Film => ctx.Films.AsNoTracking().FirstOrDefault(f => f.Name == filmDrop.captionText.text),
        };
        SetFilm(film, source is Source.Film);
        UpdateOptions();
    }

    void UpdateOptions()
    {
        var currentText = filmDrop.captionText.text;
        using var ctx = new CalenderContext();
        var filmNames = ctx.Films.Select(fp => fp.Name).ToList();
        filmDrop.ClearOptions();
        filmDrop.AddOptions(filmNames);
        int index = filmDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == currentText);
        if (index > -1) filmDrop.SetValueWithoutNotify(index);
        filmDrop.RefreshShownValue();
    }

    void SetFilm(Film film, bool fromItself = true)
    {
        if (film is null) return;
        currentId = film.Id;
        filmName.Val = film.Name;
        if (!fromItself)
        {
            int index = filmDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == film.Name);
            filmDrop.SetValueWithoutNotify(index);
        }
    }

    void ClearFilm()
    {
        currentId = null;
        filmName.Val = "";
    }

    void AddData()
    {
        using var ctx = new CalenderContext();
        var name = "Пленка " + DateTime.Now.ToString("hh:mm");
        var film = new Film() { Name = name };
        ctx.Films.Add(film);
        ctx.SaveChanges();
        SetFilm(film);
        UpdateOptions();
        int index = filmDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == film.Name);
        if (index > -1) filmDrop.SetValueWithoutNotify(index);
    }

    void SaveData()
    {
        if (currentId is null) return;
        using var ctx = new CalenderContext();
        var film = ctx.Films.Find(currentId);
        film.Name = (string)filmName.Val;
        ctx.SaveChanges();
        UpdateOptions();
    }

    void DeleteData(string name)
    {
        if (string.IsNullOrEmpty(name)) return;
        using var ctx = new CalenderContext();
        var film = ctx.Films.First(c => c.Name == name);
        if (currentId == film.Id) ClearFilm();
        ctx.Films.Remove(film);
        ctx.SaveChanges();
        UpdateOptions();
        filmDrop.Refresh();
    }

    public void RemoveData(Button remBtn)
    {
        string optText = remBtn.transform.parent.GetComponentInChildren<TMP_Text>().text;
        DeleteData(optText);
    }
}
