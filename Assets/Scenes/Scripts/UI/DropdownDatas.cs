using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DropdownDatas : MonoBehaviour
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

    private Dictionary<string, Value> currVals;
    [SerializeField] private Dictionary<string, Dictionary<string, object>> optVals = new();
    
    [SerializeField] private string newOptionText = "Новый";

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button addBtn;
    [SerializeField] private Button saveBtn;

    void OnEnable()
    {
        if (isInit) return;
        isInit = true;

        StringValue strVal = inputField.GetComponentInParent<StringValue>();
        designValName = strVal.name;

        currVals = GetComponentInParent<ValuesGroup>().GetVals();
        currVals[designValName] = strVal;

        addBtn.onClick.AddListener(() => AddOption(newOptionText));
        saveBtn.onClick.AddListener(() => SaveVals(dropdown.captionText.text));

        dropdown.onValueChanged.AddListener((int _) => InitVals());
        dropdown.ClearOptions();

        SetData(GetComponentInParent<CollectData>().GetData());
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

            SetVals(optVal);
        }
    }

    private void AddOption(string name)
    {
        if (name != string.Empty)
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
                
            name = ValidName(name);
            dropdown.AddOptions(new List<string> { name });

            ResetOptVals(name);
            dropdown.value = dropdown.options.Count - 1;
            if (dropdown.options.Count == 1)
            {
                InitVals();
            }

            inputField.Select();
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

    private bool IsValsChanged()
    {
        if (!optVals.ContainsKey(CurrOptText))
            return false;

        var optVal = optVals[CurrOptText];
        foreach (var val in currVals)
        {
            if (!optVal[val.Key].Equals(val.Value.Val))
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
            if (currVals[valKey] is StringValue)
            {
                optVal[valKey] = "";
            }
            else if (currVals[valKey] is DropDownValue)
            {
                optVal[valKey] = 1;
            }
            else if (currVals[valKey] is DoubleValue)
            {
                optVal[valKey] = 0d;
            }
            else
            {
                optVal[valKey] = 0;
            }
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
