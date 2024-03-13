using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour
{
    class Tab
    {
        public Tab(Button btn, GameObject tab)
        {
            this.btn = btn;
            this.tab = tab;
        }

        public Button btn;
        public GameObject tab;
    }

    [SerializeField] private Button addBtn;
    [SerializeField] private Button removeBtn;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject tabPrefab;
    [SerializeField] private Button tabBtnPrefab;

    private List<Tab> tabs = new List<Tab>();
    private Tab currTab = null;

    void Start()
    {
        tabPrefab.SetActive(false);
        tabBtnPrefab.gameObject.SetActive(false);

        addBtn.onClick.AddListener(() =>
        {
            AddTab();
        });

        removeBtn.onClick.AddListener(() =>
        {
            RemoveTab(tabs.Count - 1);
        });

        AddTab();
    }

    void AddTab()
    {
        GameObject newTab = Instantiate(tabPrefab);
        newTab.transform.SetParent(tabPrefab.transform.parent.transform, false);
        newTab.SetActive(false);

        GameObject newTabBtn = Instantiate(tabBtnPrefab.gameObject);
        newTabBtn.transform.SetParent(tabBtnPrefab.gameObject.transform.parent.transform, false);
        newTabBtn.SetActive(true);
        int index = tabs.Count;
        newTabBtn.GetComponentInChildren<TMP_Text>().text = (index + 1).ToString();

        Button btn = newTabBtn.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            SetActiveTab(index);
        });

        tabs.Add(new Tab(btn, newTab));

        SetActiveTab(index);
    }

    void RemoveTab(int index)
    {
        if (index <= 0) return;

        if (currTab == tabs[index])
        {
            SetActiveTab(index - 1);
        }

        Destroy(tabs[index].tab);
        Destroy(tabs[index].btn.gameObject);
        tabs.RemoveAt(index);
    }

    void SetActiveTab(int index)
    {
        if (currTab != null)
        {
            currTab.tab.SetActive(false);
            currTab.btn.GetComponent<Image>().color = Color.gray;
        }

        currTab = tabs[index];
        currTab.tab.SetActive(true);
        scrollRect.content = currTab.tab.GetComponent<RectTransform>();
        currTab.btn.GetComponent<Image>().color = Color.white;
    }
}
