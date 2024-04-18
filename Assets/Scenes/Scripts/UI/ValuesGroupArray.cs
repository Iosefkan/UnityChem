using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ValuesGroupArray : MonoBehaviour
{
    public string name;
    private TabsManager tabsManager;

    void OnEnable()
    {
        if (tabsManager == null) tabsManager = GetComponent<TabsManager>();
    }

    public List<ValuesGroup> GetGroups()
    {
        return GetComponentsInChildren<ValuesGroup>().ToList();
    }

    public List<ValuesGroup> GetCurrentGroups()
    {
        return tabsManager.GetCurrentTab().tab.GetComponents<ValuesGroup>().ToList();
    }

    public void SetSize(int size)
    {
        while (tabsManager.TabsCount() < size)
        {
            tabsManager.AddTab();
        }
        while (tabsManager.TabsCount() > size)
        {
            tabsManager.RemoveLast();
        }
    }
}
