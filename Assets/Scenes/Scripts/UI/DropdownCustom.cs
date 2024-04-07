using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.tvOS;
using UnityEngine.UI;

public class DropdownCustom : MonoBehaviour
{
    private enum TypeEdit
    {
        Edit,
        Add
    }

    bool isInit = false;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField field;
    [SerializeField] private Button addBtn;
    //[SerializeField] private Button remBtn;
    [SerializeField] private Button editBtn;

    private TypeEdit typeEdit;
    
    void OnEnable()
    {
        if (isInit) return;
        isInit = true;

        //remBtn.onClick.AddListener(()=>RemoveOption(remBtn));
        addBtn.onClick.AddListener(() => StartEdit(TypeEdit.Add));
        editBtn.onClick.AddListener(() => StartEdit(TypeEdit.Edit));
        field.onEndEdit.AddListener(EndEdit);
    }

    private void StartEdit(TypeEdit type)
    {
        if (type == TypeEdit.Edit && dropdown.options.Count == 0)
        {
            return;
        }

        typeEdit = type;
        field.text = dropdown.captionText.text;
        field.gameObject.SetActive(true);
        field.Select();
    }

    private void EndEdit(string val)
    {
        if (val != string.Empty)
        {
            if (typeEdit == TypeEdit.Edit)
            {
                dropdown.captionText.text = val;
                dropdown.options[dropdown.value].text = val;
            }
            else if (typeEdit == TypeEdit.Add)
            {
                dropdown.AddOptions(new List<string> { val });
                dropdown.value = dropdown.options.Count - 1;
            }
        }

        field.gameObject.SetActive(false);
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
