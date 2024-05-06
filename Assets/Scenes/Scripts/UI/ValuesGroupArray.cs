using Assets.Scenes.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ValuesGroupArray : Value
{
    private TabsManager tabsManager;

    void Awake()
    {
        tabsManager = GetComponent<TabsManager>();
    }

    public override object Val
    {
        get
        {
            var arr = GetDropdownDatasVal();
            MyList<string> vals = new MyList<string>();
            foreach (var item in arr)
            {
                vals.Add(item.Val.ToString());
            }
            return vals;
        }
        set
        {
            if(value is List<string>)
                SetVal(value as List<string>);
            if (value is MyList<string>)
                SetVal(value as MyList<string>);
            //else
            //    Debug.Log("В ValuesGroupArray был передан не правильный массив!");
        }
    }

    private void SetVal(MyList<string> otherDDs)
    {
        //if (otherDDs == null)
        //{
        //    SetSize(1);
        //}

        SetSize(otherDDs.Count);
        MyList<Value> dds = GetDropdownDatasVal();
        for (int i = 0; i < otherDDs.Count; i++)
        {
            dds[i].Val = otherDDs[i];
        }
    }

    private void SetVal(List<string> otherDDs)
    {
        SetSize(otherDDs.Count);
        MyList<Value> dds = GetDropdownDatasVal();
        for (int i = 0; i < otherDDs.Count; i++)
        {
            dds[i].Val = otherDDs[i];
        }
    }

    public override object DefaultVal
    {
        get
        {
            string def = "";
            var arr = GetDropdownDatasVal();
            if (arr.Count > 0)
            {
                def = arr.Last().Val.ToString();
            }
            return new MyList<string>() { def }; 
        }
    }

    public List<ValuesGroup> GetGroups()
    {
        return GetComponentsInChildren<ValuesGroup>().ToList();
    }

    public List<ValuesGroup> GetCurrentGroups()
    {
        return tabsManager.GetCurrentTab().tab.GetComponents<ValuesGroup>().ToList();
    }

    public MyList<Value> GetDropdownDatasVal()
    {
        MyList<Value> arr = new();
        arr.AddRange(GetComponentsInChildren<DropdownDatas>());
        return arr;
    }

    public void SetSize(int size)
    {
        if (size <= 0) size = 1;

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
