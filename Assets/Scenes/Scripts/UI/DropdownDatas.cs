using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DropdownDatas : Value
{
    private bool isInit = false;

    private int currOptIndex = 0;
    private string CurrOptText
    {
        get
        {
            return dropdown.options.Count > currOptIndex ? 
                        dropdown.options[currOptIndex].text :
                        "";
        }
        set
        {
            if (dropdown.options.Count > currOptIndex)
            {
                dropdown.options[currOptIndex].text = value;
            }
        }
    }

    private string designValName = string.Empty;

    private Dictionary<string, Value> currVals = new();
    private Dictionary<string, Dictionary<string, object>> optVals = new();
    
    [SerializeField] private string newOptionText = "Новый";

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button addBtn;
    [SerializeField] private Button saveBtn;

    [SerializeField] private List<ValuesGroup> valGrps;
    [SerializeField] private List<ValuesGroupArray> valGrpsArr;
    [SerializeField] private List<DropdownDatas> valDropdowns;

    void Awake()
    {
        StringValue strVal = inputField.GetComponentInParent<StringValue>();
        designValName = strVal.name;

        currVals[designValName] = strVal;

        foreach (var valGrp in valGrps)
        {
            currVals.AddRange(valGrp.GetVals());
        }

        foreach (var valArrGrp in valGrpsArr)
        {
            currVals[valArrGrp.name] = valArrGrp;
        }

        foreach (var dd in valDropdowns)
        {
            currVals[dd.name] = dd;
        }

        addBtn.onClick.AddListener(() =>
        {
            if (IsValsChanged())
            {
                if (ResetChangesMsg())
                {
                    ResetChanges();
                }
                else
                {
                    return;
                }
            }

            AddOption(newOptionText);

            dropdown.value = dropdown.options.Count - 1;
            if (dropdown.options.Count == 1)
            {
                InitVals();
            }

            inputField.Select();
        });
        saveBtn.onClick.AddListener(() => SaveVals(dropdown.captionText.text));

        dropdown.onValueChanged.AddListener((int _) => InitVals());
        dropdown.ClearOptions();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (!isInit)
        {
            SetData(GetComponentInParent<CollectData>().GetData(name));
            isInit = true;
        }
    }

    public override object Val
    {
        get
        {
            Init();
            if (IsValsChanged())
                return -1;
            return dropdown.value;
        }
        set
        {
            Init();
            dropdown.value = (int)value;
        }
    }

    public void SetData(List<Dictionary<string, object>> data)
    {
        foreach (var vals in data)
        {
            string designName = (string)vals[designValName];
            AddOption(designName);
            var optVal = optVals[designName];
            foreach (var item in vals)
            {
                optVal[item.Key] = item.Value;
            }
        }

        if (optVals.Count > 0)
        {
            SetVals(optVals.First().Value);
        }
    }

    private void AddOption(string name)
    {
        if (name != string.Empty)
        {
            name = ValidName(name);
            dropdown.AddOptions(new List<string> { name });

            ResetOptVals(name);
        }
    }

    private void InitVals()
    {
        if (currOptIndex != dropdown.value && IsValsChanged())
        {
            if (!ResetChangesMsg())
            {
                dropdown.SetValueWithoutNotify(currOptIndex);
                return;
            }
        }

        currOptIndex = dropdown.value;
        SetVals(optVals[dropdown.captionText.text]);
    }

    public bool IsValsChanged()
    {
        if (!optVals.ContainsKey(CurrOptText))
            return false;

        var optVal = optVals[CurrOptText];
        foreach (var val in currVals)
        {
            if (!val.Value.Val.Equals(optVal[val.Key]))
                return true;
        }

        return false;
    }

    private void SaveVals(string optName)
    {
        string newOptName = (string)currVals[designValName].Val;
        if (newOptName != optName && optVals.ContainsKey(newOptName))
        {
            EditorUtility.DisplayDialog("Предупреждение", $"Название {newOptName} уже используется!\n Попробуйте другое.", "Хорошо");
            return;
        }

        var optVal = optVals[optName];
        foreach (var valName in currVals.Keys)
        {
            optVal[valName] = currVals[valName].Val;
        }

        if (optName != newOptName)
        {
            optVals.Remove(optName);
            optVals[newOptName] = optVal;
        }

        dropdown.options[dropdown.value].text = newOptName;
        dropdown.captionText.text = newOptName;
    }

    private void ResetOptVals(string optName)
    {
        if (!optVals.ContainsKey(optName))
        {
            optVals[optName] = new Dictionary<string, object>();
        }

        var optVal = optVals[optName];
        List<string> keys = new List<string>(currVals.Keys);
        foreach (var valKey in keys)
        {
            optVal[valKey] = currVals[valKey].DefaultVal;
        }

        optVal[designValName] = optName;
    }

    private void ResetChanges()
    {
        var optVal = optVals[CurrOptText];
        List<string> keys = new List<string>(currVals.Keys);
        foreach (var valKey in keys)
        {
            currVals[valKey].Val = optVal[valKey];
        }
    }

    private void SetVals(Dictionary<string, object> newVals)
    {
        foreach (var val in newVals)
        {
            if (currVals.ContainsKey(val.Key))
            {
                currVals[val.Key].Val = val.Value;
            }
        }
    }

    public void RemoveOption(Button remBtn)
    {
        string optText = remBtn.transform.parent.GetComponentInChildren<TMP_Text>().text;
        int index = dropdown.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == optText);

        dropdown.options.RemoveAt(index);
        optVals.Remove(optText);

        if (index <= currOptIndex)
        {
            currOptIndex = Math.Max(currOptIndex - 1, 0);
        }
        if (index <= dropdown.value)
        {
            dropdown.value -= 1;
        }

        if (dropdown.options.Count == 0)
        {
            AddOption(newOptionText);
        }

        RefreshOptions();
    }

    public void RefreshOptions()
    {
        dropdown.enabled = false;
        dropdown.enabled = true;
        dropdown.Show();
    }

    private string ValidName(string name)
    {
        string postfix = "";
        int postfixVal = 0;
        while (optVals.ContainsKey(name + postfix))
        {
            postfix = (++postfixVal).ToString();
        }

        return name +postfix;
    }

    private bool ResetChangesMsg()
    {
        return EditorUtility.DisplayDialog("Предупреждение", "Изменения не сохранены, продолжить?", "Да", "Нет");
    }
}
