using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    [Serializable]
    class Column
    {
        [SerializeField]
        public string header = "";
        [SerializeField]
        public int numberPrecision = -1;
    }

    private bool isInit = false;

    private List<GameObject> gameObjectList = new List<GameObject>();

    [SerializeField] private GameObject rowContainer;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private List<Column> columns = new();
    //[SerializeField] private List<string> headers = new List<string>() { "" };
    //[SerializeField] private int numberPrecision = -1;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        if (!isInit)
        {
            isInit = true;
            rowPrefab.SetActive(false);
            cellPrefab.SetActive(false);
            GameObject newRow = AddRow(ToHeaderList(columns));
            TMP_Text[] texts = newRow.GetComponentsInChildren<TMP_Text>();
            foreach(var text in texts)
            {
                text.fontStyle = FontStyles.Bold;
            }
        }
    }

    List<string> ToHeaderList(List<Column> columnList)
    {
        List<string> headers = new List<string>();
        foreach(var column in columnList)
        {
            headers.Add(column.header);
        }
        return headers;
    }

    public void SetData<T>(List<List<T>> valueList)
    {
        Init();
        ClearObjs();
        AddData(valueList);
    }

    public void Clear()
    {
        Init();
        ClearObjs();
    }

    public void AddData<T>(List<List<T>> valueList)
    {
        Init();
        foreach (var rowList in valueList)
        {
            gameObjectList.Add(AddRow(rowList));
        }
    }

    public void AddData<T>(List<T> valueList)
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

    GameObject AddRow<T>(List<T> rowList)
    {
        GameObject newRow = AddElement(rowPrefab, rowContainer);
        int i = 0;
        foreach (var val in rowList)
        {
            GameObject newCell = AddElement(cellPrefab, newRow);
            if (columns[i].numberPrecision != -1 && (val.GetType() == typeof(double) || val.GetType() == typeof(float)))
            {
                newCell.GetComponentInChildren<TMP_Text>().text = string.Format("{0:f" + columns[i].numberPrecision + "}", val);
            }
            else
            {
                newCell.GetComponentInChildren<TMP_Text>().text = val.ToString();
            }
            i++;
        }

        return newRow;
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
