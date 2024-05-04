using Assets.Scenes.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

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
            SetVal(value as MyList<string>);
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

    public override object DefaultVal
    {
        get { return new MyList<string>() { "" }; }
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
