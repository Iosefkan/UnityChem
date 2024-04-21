using Assets.Scenes.Scripts.UI;
using Program;
using System.Collections.Generic;
using System.Linq;
using Types;
using UnityEngine;

public class CollectData : MonoBehaviour
{
    private List<ValuesGroup> valuesGroups;
    private List<ValuesGroupArray> valuesGroupArrays;

    private Dictionary<string, Dictionary<string, object>> datas = new();

    private void Start()
    {
        //SetInitData(new InitData());
    }

    //public List<Dictionary<string, object>> GetData(string name)
    //{
    //    var l = new List<Dictionary<string, object>>()
    //    {
    //        new(), new(), new(), new()
    //    };
    //    if (name == "BodySection")
    //    {
    //        var filds = typeof(CYLINDER).GetFields();
    //        CYLINDER[] cyl = new CYLINDER[4];
    //        cyl[0].Var_T = 2;
    //        cyl[1].Var_T = 1;
    //        cyl[2].Var_T = 2;
    //        cyl[3].Var_T = 1;
    //        cyl[0].L_sec = 123;
    //        cyl[1].L_sec = 432;
    //        cyl[2].L_sec = 67583456;
    //        cyl[3].L_sec = 234532452356;
    //        foreach (var fild in filds)
    //        {
    //            l[0][fild.Name] = fild.GetValue(cyl[0]);
    //            l[1][fild.Name] = fild.GetValue(cyl[1]);
    //            l[2][fild.Name] = fild.GetValue(cyl[2]);
    //            l[3][fild.Name] = fild.GetValue(cyl[3]);
    //        }

    //        l[0]["Designation"] = "1";
    //        l[1]["Designation"] = "2";
    //        l[2]["Designation"] = "3";
    //        l[3]["Designation"] = "4";
    //    }
    //    else if (name == "Body")
    //    {
    //        l[0]["Lam_k"] = 1111d;
    //        l[0]["Types.CYLINDER"] = new MyList<int>() { 0, 1, 0, 1 };
    //        l[0]["Designation"] = "sec1";
    //        l[1]["Lam_k"] = 2222d;
    //        l[1]["Types.CYLINDER"] = new MyList<int>() { 1, 0, 0, 0 };
    //        l[1]["Designation"] = "sec2";
    //        l[2]["Lam_k"] = 3333d;
    //        l[2]["Types.CYLINDER"] = new MyList<int>() { 1, 1, 1, 1 };
    //        l[2]["Designation"] = "sec3";
    //        l[3]["Lam_k"] = 44444d;
    //        l[3]["Types.CYLINDER"] = new MyList<int>() { 1,0,2,1 };
    //        l[3]["Designation"] = "sec4";
    //    }

    //    return l;
    //}

    public InitData GetInitData()
    {
        valuesGroups = new List<ValuesGroup>(GetComponentsInChildren<ValuesGroup>());
        valuesGroupArrays = new List<ValuesGroupArray>(GetComponentsInChildren<ValuesGroupArray>());

        InitData initData = new InitData(false);
        InitValuesGroupArray(ref initData.cyl, "CYLINDER_CONF");
        InitValuesGroupArray(ref initData.sect, "SECT_CONF");
        InitValuesGroupArray(ref initData.S_K.S, "SECTIONS_CONF");
        initData.S_K.Num_S = initData.S_K.S.Length;
        InitValuesGroup(ref initData.data);
        initData.data.nS_1 = initData.sect.Count((SECT sec) => sec.S_Type == 1);
        initData.data.nS_2 = initData.sect.Count((SECT sec) => sec.S_Type == 2);
        initData.data.nS_korp = initData.cyl.Count();
        InitValuesGroup(ref initData.dop);
        InitValuesGroup(ref initData.fluxData);
        InitValuesGroup(ref initData.train);

        return initData;
    }

    public void SetInitData(InitData initData)
    {
        List<ValuesGroupArray> arrays = new List<ValuesGroupArray>(GetComponentsInChildren<ValuesGroupArray>());
        AbjastValuesGroupArray(arrays, initData.cyl);
        AbjastValuesGroupArray(arrays, initData.sect);
        AbjastValuesGroupArray(arrays, initData.S_K.S);

        valuesGroups = new List<ValuesGroup>(GetComponentsInChildren<ValuesGroup>());
        valuesGroupArrays = new List<ValuesGroupArray>(GetComponentsInChildren<ValuesGroupArray>());

        SetValuesGroupArrays(initData.cyl);
        SetValuesGroupArrays(initData.sect);
        SetValuesGroupArrays(initData.S_K.S);
        SetValuesGroup(initData.data);
        SetValuesGroup(initData.dop);
        SetValuesGroup(initData.fluxData);
        SetValuesGroup(initData.train);
    }

    void AbjastValuesGroupArray<T>(List<ValuesGroupArray> arrays, T[] vals)
    {
        var name = typeof(T).ToString();
        foreach (var arr in arrays)
        {
            if (arr.name == name)
            {
                arr.SetSize(vals.Length);
            }
        }
    }

    List<ValuesGroup> GetValuesGroups(string name)
    {
        List<ValuesGroup> grs = new List<ValuesGroup>();
        for (int i = 0; i < valuesGroups.Count; ++i)
        {
            if (valuesGroups[i] is ValuesGroup &&
                valuesGroups[i].name == name)
            {
                grs.Add(valuesGroups[i]);
            }
        }

        if (grs.Count == 0)
        {
            //Debug.Log($"На форме нет группы {name}");
        }

        return grs;
    }

    ValuesGroupArray GetValuesGroupArray(string name)
    {
        for (int i = 0; i < valuesGroupArrays.Count; ++i)
        {
            if (valuesGroupArrays[i].name == name)
            {
                return valuesGroupArrays[i];
            }
        }

        return null;
    }

    public void InitValuesGroupArray<T>(ref T[] valuesGroup, string name)
    {
        ValuesGroupArray grArrs = GetValuesGroupArray(name);
        List<ValuesGroup> grs = grArrs.GetGroups();
        if (grs.Count == 0)
        {
            //Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        valuesGroup = new T[grs.Count];
        for (int i = 0; i < grs.Count; ++i)
        {
            InitValuesGroupFromGroup(ref valuesGroup[i], grs.GetRange(i, 1));
        }
    }

    public void InitValuesGroup<T>(ref T valuesGroup)
    {
        List<ValuesGroup> grs = GetValuesGroups(typeof(T).ToString()[6..]);
        if (grs.Count == 0)
        {
            Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        InitValuesGroupFromGroup(ref valuesGroup, grs);
    }

    public void InitValuesGroupFromGroup<T>(ref T valuesGroup, List<ValuesGroup> grs)
    {
        var filds = typeof(T).GetFields();
        for (int i = 0; i < grs.Count; ++i)
        {
            var vals = grs[i].GetVals();
            foreach (var fild in filds)
            {
                if (fild.Name == "Order")
                {
                    object obj1 = valuesGroup;
                    fild.SetValue(obj1, i + 1);
                    valuesGroup = (T)obj1;
                }

                if (vals.ContainsKey(fild.Name))
                {
                    object obj = valuesGroup;
                    fild.SetValue(obj, vals[fild.Name].Val);
                    valuesGroup = (T)obj;
                }
            }
        }
    }

    public void SetValuesGroupArrays<T>(T[] valuesGroup)
    {
        ValuesGroupArray grArrs = GetValuesGroupArray(typeof(T).ToString());
        List<ValuesGroup> grs = grArrs.GetGroups();
        if (grs.Count == 0)
        {
            //Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        for (int i = 0; i < grs.Count(); ++i)
        {
            SetValuesGroupFromGroup(valuesGroup[i], grs.GetRange(i, 1));
        }
    }

    public void SetValuesGroup<T>(T valuesGroup)
    {
        List<ValuesGroup> grs = GetValuesGroups(typeof(T).ToString());
        if (grs.Count == 0)
        {
            //Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        SetValuesGroupFromGroup(valuesGroup, grs);
    }

    public void SetCurrentValuesGroup<T>(T valuesGroup)
    {
        ValuesGroupArray grArrs = GetValuesGroupArray(typeof(T).ToString());
        List<ValuesGroup> grs = grArrs.GetCurrentGroups();
        if (grs.Count == 0)
        {
            //Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        SetValuesGroupFromGroup(valuesGroup, grs);
    }

    public void SetValuesGroupFromGroup<T>(T valuesGroup, List<ValuesGroup> grs)
    {
        var filds = typeof(T).GetFields();
        for (int i = 0; i < grs.Count; ++i)
        {
            var vals = grs[i].GetVals();
            foreach (var fild in filds)
            {
                if (vals.ContainsKey(fild.Name))
                {
                    vals[fild.Name].Val = fild.GetValue(valuesGroup);
                }
            }
        }
    }
}
