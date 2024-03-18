using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    private List<GameObject> gameObjectList = new List<GameObject>();

    [SerializeField] private GameObject rowContainer;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private List<string> headers = new List<string>() { "" };
    [SerializeField] private int numberPrecision = -1;

    void Awake()
    {
        rowPrefab.SetActive(false);
        cellPrefab.SetActive(false);
        GameObject newRow = AddRow(headers);
    }

    public void SetData<T>(List<List<T>> valueList)
    {
        ClearObjs();
        AddData(valueList);
    }

    public void AddData<T>(List<List<T>> valueList)
    {
        foreach (var rowList in valueList)
        {
            gameObjectList.Add(AddRow(rowList));
        }
    }

    public void AddData<T>(List<T> valueList)
    {
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
        foreach (var val in rowList)
        {
            GameObject newCell = AddElement(cellPrefab, newRow);
            if (numberPrecision != -1 && (val.GetType() == typeof(double) || val.GetType() == typeof(float)))
            {
                newCell.GetComponentInChildren<TMP_Text>().text = string.Format("{0:f" + numberPrecision + "}", val);
            }
            else
            {
                newCell.GetComponentInChildren<TMP_Text>().text = val.ToString();
            }
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
