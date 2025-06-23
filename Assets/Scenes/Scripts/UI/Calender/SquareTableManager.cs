using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquareTableManager : MonoBehaviour
{

    [SerializeField] private int numberPrecision = 2;
    [SerializeField] private Color cellColor;

    private List<GameObject> gameObjectList = new List<GameObject>();
    private int rowCount = 0;

    [SerializeField] private GameObject rowContainer;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject cellPrefab;

    private void Awake()
    {
        rowPrefab.SetActive(false);
        cellPrefab.SetActive(false);
    }

    public void SetData<T>(List<List<T>> valueList)
    {
        ClearObjs();
        AddData(valueList);
    }

    public void Clear()
    {
        ClearObjs();
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

        for (int i = 0; i < rowList.Count; i++)
        {
            GameObject newCell = AddElement(cellPrefab, newRow);
            if (numberPrecision >= 0 && (rowList[i].GetType() == typeof(double) || rowList[i].GetType() == typeof(float)))
            {
                var cellText = newCell.GetComponentInChildren<TMP_Text>();
                cellText.text = string.Format("{0:f" + numberPrecision + "}", rowList[i]);
                if (rowCount == 0)
                {
                    cellText.fontStyle = FontStyles.Bold;
                }
            }
            else
            {
                var cellText = newCell.GetComponentInChildren<TMP_Text>();
                cellText.text = rowList[i].ToString();
                if (rowCount == 0)
                {
                    cellText.fontStyle = FontStyles.Bold;
                }
            }

            if (i == 0 && rowCount > 0)
            {
                newCell.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Bold;
            }
        }
        rowCount++;

        return newRow;
    }

    public void SetColor(int top, int bottom, int left, int right)
    {
        for (int i = top; i <= bottom; i++)
        {
            var row = gameObjectList[i + 1];
            for (int j = left; j <= right; j++)
            {
                var cell = row.transform.GetChild(j + 2).gameObject;
                var image = cell.GetComponent<Image>();
                image.color = cellColor;
            }
        }
    }

    void ClearObjs()
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        rowCount = 0;
    }
}
