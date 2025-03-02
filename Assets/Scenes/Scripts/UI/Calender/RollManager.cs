using CalenderDatabase;
using Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class RollManager : MonoBehaviour
{
    public StringValue rollName;
    public TMP_Dropdown rollDrop;
    public TMP_Dropdown scenarioDrop;
    public Button addRoll;
    public Button saveRoll;

    public DoubleValue firstDistance;
    public DoubleValue secondDistance;
    public DoubleValue width;
    public DoubleValue elasticity;
    public DoubleValue barrelDiameter;
    public DoubleValue neckDiameter;
    public DoubleValue holeDiameter;

    private long? currentId;

    private void Awake()
    {
        rollDrop.onValueChanged.AddListener(_ => LoadData(Source.Roll));
        scenarioDrop.onValueChanged.AddListener(_ => LoadData(Source.Scenario));
        UpdateOptions();

        addRoll.onClick.AddListener(AddData);
        saveRoll.onClick.AddListener(SaveData);
    }

    enum Source
    {
        Scenario,
        Roll
    }

    void LoadData(Source source)
    {
        using var ctx = new CalenderContext();
        var roll = source switch
        {
            Source.Scenario => ctx.Scenarios.Include(fp => fp.RollSettings).AsNoTracking().FirstOrDefault(s => s.Name == scenarioDrop.captionText.text).RollSettings,
            Source.Roll => ctx.RollSettings.AsNoTracking().FirstOrDefault(f => f.Name == rollDrop.captionText.text),
        };
        SetRoll(roll, source is Source.Roll);
        UpdateOptions();
    }

    void UpdateOptions()
    {
        var currentText = rollDrop.captionText.text;
        using var ctx = new CalenderContext();
        var rollNames = ctx.RollSettings.Select(fp => fp.Name).ToList();
        rollDrop.ClearOptions();
        rollDrop.AddOptions(rollNames);
        int index = rollDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == currentText);
        if (index > -1) rollDrop.SetValueWithoutNotify(index);
        //else rollDrop.onValueChanged.Invoke(0);
        rollDrop.RefreshShownValue();
    }

    void SetRoll(RollSetting roll, bool fromItself = true)
    {
        if (roll is null) return;
        currentId = roll.Id;
        firstDistance.Val = roll.FirstDistance;
        secondDistance.Val = roll.SecondDistance;

        width.Val = roll.Width;

        elasticity.Val = roll.Elasticity;

        neckDiameter.Val = roll.NeckDiameter;
        holeDiameter.Val = roll.HoleDiameter;
        barrelDiameter.Val = roll.BarrelDiameter;

        rollName.Val = roll.Name;

        if (!fromItself)
        {
            int index = rollDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == roll.Name);
            rollDrop.SetValueWithoutNotify(index);
        }
    }

    void ClearRoll()
    {
        currentId = null;
        firstDistance.Val = 0d;
        secondDistance.Val = 0d;
        elasticity.Val = 0;
        width.Val = 0d;
        neckDiameter.Val = 0d;
        holeDiameter.Val = 0d;
        barrelDiameter.Val = 0d;
        rollName.Val = "";
    }

    void AddData()
    {
        using var ctx = new CalenderContext();
        var name = "Валок " + DateTime.Now.ToString("hh:mm");
        var roll = new RollSetting() { Name = name };
        ctx.RollSettings.Add(roll);
        ctx.SaveChanges();
        SetRoll(roll);
        UpdateOptions();
        int index = rollDrop.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == roll.Name);
        if (index > -1) rollDrop.SetValueWithoutNotify(index);
    }

    void SaveData()
    {
        if (currentId is null) return;
        using var ctx = new CalenderContext();
        var roll = ctx.RollSettings.Find(currentId);

        roll.FirstDistance = (double)firstDistance.Val;
        roll.SecondDistance = (double)secondDistance.Val;

        roll.Width = (double)width.Val;
        roll.Elasticity = (double)elasticity.Val;
        roll.NeckDiameter = (double)neckDiameter.Val;
        roll.HoleDiameter = (double)holeDiameter.Val;
        roll.BarrelDiameter = (double)barrelDiameter.Val;
        roll.Name = (string)rollName.Val;

        ctx.SaveChanges();
        UpdateOptions();
    }

    void DeleteData(string name)
    {
        if (string.IsNullOrEmpty(name)) return;
        using var ctx = new CalenderContext();
        var roll = ctx.RollSettings.First(c => c.Name == name);
        if (currentId == roll.Id) ClearRoll();
        ctx.RollSettings.Remove(roll);
        ctx.SaveChanges();
        UpdateOptions();
        rollDrop.Refresh();
    }

    public void RemoveData(Button remBtn)
    {
        string optText = remBtn.transform.parent.GetComponentInChildren<TMP_Text>().text;
        DeleteData(optText);
    }
}
