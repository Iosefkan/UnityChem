using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    private List<GameObject> gameObjectList = new List<GameObject>();

    public List<string> headers = new List<string>() { "" };
    public GameObject container;
    private GameObject rowPrefab;
    private GameObject cellPrefab;

    void Awake()
    {
        rowPrefab = container.transform.GetChild(0)?.gameObject;
        cellPrefab = rowPrefab.transform.GetChild(0)?.gameObject;

        GameObject newRow = Instantiate(rowPrefab);
        newRow.transform.SetParent(container.transform, false);
        newRow.SetActive(true);
        foreach (var val in headers)
        {

            GameObject newCell = Instantiate(cellPrefab);
            newCell.transform.SetParent(newRow.transform, false);
            newCell.SetActive(true);
            newCell.GetComponentInChildren<TMP_Text>().text = val;
        }
    }

    public void Show(List<List<string>> valueList)
    {
        ClearObjs();

        foreach (var rowList in valueList)
        {
            AddRow(rowList);
        }
    }

    GameObject AddElement(GameObject prefab, GameObject parent)
    {
        GameObject newEl = Instantiate(prefab);
        newEl.transform.SetParent(parent.transform, false);
        newEl.SetActive(true);
        gameObjectList.Add(newEl);

        return newEl;
    }

    void AddRow(List<string> rowList)
    {
        GameObject newRow = AddElement(rowPrefab, container);
        foreach (var val in rowList)
        {
            GameObject newCell = AddElement(cellPrefab, newRow);
            newCell.GetComponentInChildren<TMP_Text>().text = val;
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
