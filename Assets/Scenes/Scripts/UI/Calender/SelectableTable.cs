using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

public class SelectableTable : MonoBehaviour
{
    [SerializeField] private Color selectedColor = new Color32(128, 182, 241, 100);
    [SerializeField] private Color baseColor = new Color32(255, 255, 255, 100);

    private bool isInit = false;
    private int? selectedId = null;
    private List<GameObject> gameObjectList = new List<GameObject>();

    [SerializeField] private GameObject rowContainer;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private List<LocalizedString> columns;
    private GameObject header;
    public List<LocalizedString.ChangeHandler> events = new List<LocalizedString.ChangeHandler>();

    private void OnEnable()
    {
        events = new();
        for (int i = 0; i < columns.Count; i++)
        {
            int var = i;
            events.Add((str) => UpdateHeaders(str, var));
            columns[i].StringChanged += events[i];
        }
    }

    private void OnDisable()
    {
        if (events.Count == 0) return;
        for (int i = 0; i < columns.Count; i++)
        {
            columns[i].StringChanged -= events[i];
        }
    }

    //[SerializeField] private List<string> columns = new() { "Логин", "Роль" };

    void UpdateHeaders(string text, int pos)
    {
        TMP_Text[] texts = header.GetComponentsInChildren<TMP_Text>();
        texts[pos].text = text;
    }

    void Awake()
    {
        Init();
        UpdateUsers();
    }

    void Init()
    {
        if (!isInit)
        {
            isInit = true;
            rowPrefab.SetActive(false);
            cellPrefab.SetActive(false);
            GameObject newRow = AddRow(columns.Select(x => x.GetLocalizedString()).ToList());
            Destroy(newRow.GetComponent<Button>());
            header = newRow;
            TMP_Text[] texts = newRow.GetComponentsInChildren<TMP_Text>();
            foreach (var text in texts)
            {
                text.fontStyle = FontStyles.Bold;
            }
        }
    }

    void UpdateUsers()
    {
        Clear();
        using var ctx = new UsersContext();
        var users = ctx.Users.Include(u => u.Role).OrderBy(u => u.Id);
        foreach (var user in users)
        {
            AddData(new List<string> { user.Login, user.Role.Name });
        }
    }

    public void DeleteSelected()
    {
        if (!selectedId.HasValue) return;
        var login = gameObjectList[selectedId.Value].transform.GetChild(1).GetComponentInChildren<TMP_Text>().text;
        using var ctx = new UsersContext();
        var user = ctx.Users.FirstOrDefault(u => u.Login == login);
        if (user is null) return;
        ctx.Users.Remove(user);
        ctx.SaveChanges();
        UpdateUsers();
    }
    
    public void AddUser(string username, string password, string roleName)
    {
        using var ctx = new UsersContext();
        var role = ctx.Roles.FirstOrDefault(u => u.Name == roleName);
        if (role is null) return;
        var user = new User() { Login = username, RoleId = role.Id, Password = password };
        ctx.Users.Add(user);
        ctx.SaveChanges();
        UpdateUsers();
    }

    void Clear()
    {
        selectedId = null;
        foreach (var row in gameObjectList)
        {
            row.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        Init();
        ClearObjs();
    }

    void AddData(List<string> valueList)
    {
        Init();
        gameObjectList.Add(AddRow(valueList));
    }

    GameObject AddElement(GameObject prefab, GameObject parent)
    {
        GameObject newEl = Instantiate(prefab);
        newEl.transform.SetParent(parent.transform, false);
        newEl.SetActive(true);

        return newEl;
    }

    GameObject AddRow(List<string> rowList)
    {
        GameObject newRow = AddElement(rowPrefab, rowContainer);
        foreach (var val in rowList)
        {
            GameObject newCell = AddElement(cellPrefab, newRow);
            newCell.GetComponentInChildren<TMP_Text>().text = val.ToString();
        }

        var number = gameObjectList.Count;
        newRow.GetComponent<Button>().onClick.AddListener(() => Select(number));

        return newRow;
    }

    void Select(int id)
    {
        if (selectedId.HasValue)
        {
            var row = gameObjectList[selectedId.Value];
            var colCount = row.transform.childCount;
            for (int i = 1; i < colCount; i++)
            {
                row.transform.GetChild(i).gameObject.GetComponent<Image>().color = baseColor;
            }
        }
        selectedId = id;
        var newRow = gameObjectList[id];
        var newColCount = newRow.transform.childCount;
        for (int i = 1; i < newColCount; i++)
        {
            newRow.transform.GetChild(i).gameObject.GetComponent<Image>().color = selectedColor;
        }
    }

    void ClearObjs()
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
    }
}
