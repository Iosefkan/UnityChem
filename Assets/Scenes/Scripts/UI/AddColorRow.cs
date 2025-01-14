using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddColorRow : MonoBehaviour
{
    [SerializeField] private GameObject intervals;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Button button;

    [SerializeField] private GameObject popup;
    [SerializeField] private ConfirmColorPopup conf;
    [SerializeField] private Image avgColor;
    public void Start()
    {
        button.onClick.AddListener(AddRow);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(AddRow);
    }

    private void AddRow()
    {
        var row = Instantiate(rowPrefab);
        row.transform.SetParent(intervals.transform, false);
        var openPicker = row.GetComponentInChildren<OpenColorPickerPopup>();
        openPicker.popup = popup;
        openPicker.conf = conf;
        openPicker.avgColor = avgColor;
        var group = intervals.GetComponent<ToggleGroup>();
        var toggle = row.GetComponentInChildren<Toggle>();
        var setToggle = group.GetFirstActiveToggle() is null;
        toggle.isOn = setToggle;
        if (setToggle) row.GetComponent<BaseSelected>().Selected(setToggle);
        toggle.group = group;
    }
}
