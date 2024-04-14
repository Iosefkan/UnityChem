using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.UI;

public class DropdownCustom : MonoBehaviour
{
    bool isInit = false;

    [SerializeField] private string newOptionText = string.Empty;

    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Button addBtn;
    [SerializeField] private Button saveBtn;

    void OnEnable()
    {
        if (isInit) return;
        isInit = true;

        addBtn.onClick.AddListener(() => AddOption(newOptionText));
    }

    private void AddOption(string val)
    {
        if (val != string.Empty)
        {
            dropdown.AddOptions(new List<string> { val });
        }
    }

    public void RemoveOption(Button remBtn)
    {
        TMP_Text text = remBtn.transform.parent.GetComponentInChildren<TMP_Text>();
        int index = dropdown.options.FindIndex((TMP_Dropdown.OptionData od) => od.text == text.text);
        
        dropdown.options.RemoveAt(index);

        if (index <= dropdown.value) dropdown.value = dropdown.value - 1;
        if (dropdown.options.Count == 0) dropdown.captionText.text = "";

        RefreshOptions();
    }

    public void RefreshOptions()
    {
        dropdown.enabled = false;
        dropdown.enabled = true;
        dropdown.Show();
    }
}
