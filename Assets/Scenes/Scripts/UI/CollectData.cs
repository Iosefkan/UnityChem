using Program;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class CollectData : MonoBehaviour
{
    private List<ValuesGroup> valuesGroups;
    private bool isInit = false;

    private void OnEnable()
    {
        if (!isInit)
        {
            SetInitData(new InitData());
            isInit = true;
        }
    }

    public InitData GetInitData()
    {
        valuesGroups = new List<ValuesGroup>(GetComponentsInChildren<ValuesGroup>());

        InitData initData = new InitData(false);
        InitDataGroup(ref initData.cyl);
        InitDataGroup(ref initData.sect);
        InitDataGroup(ref initData.S_K.S);
        initData.S_K.Num_S = initData.S_K.S.Length;
        InitDataGroup(ref initData.data);
        initData.data.nS_1 = initData.sect.Count((Types.SECT sec) => sec.S_Type == 1);
        initData.data.nS_2 = initData.sect.Count((Types.SECT sec) => sec.S_Type == 2);
        initData.data.nS_korp = initData.cyl.Count();
        InitDataGroup(ref initData.dop);
        InitDataGroup(ref initData.fluxData);
        InitDataGroup(ref initData.train);

        return initData;
    }

    public void SetInitData(InitData initData)
    {
        List<ValuesGroupArray>  arrays = new List<ValuesGroupArray>(GetComponentsInChildren<ValuesGroupArray>());
        AbjastValuesGroupArray(arrays, initData.cyl);
        AbjastValuesGroupArray(arrays, initData.sect);
        AbjastValuesGroupArray(arrays, initData.S_K.S);

        valuesGroups = new List<ValuesGroup>(GetComponentsInChildren<ValuesGroup>());
        SetInitData(initData.cyl);
        SetInitData(initData.sect);
        SetInitData(initData.S_K.S);
        SetInitData(initData.data);
        SetInitData(initData.dop);
        SetInitData(initData.fluxData);
        SetInitData(initData.train);
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

    List<ValuesGroup> GetValuesGroup(string name)
    {
        List<ValuesGroup> grs = new List<ValuesGroup>();
        for (int i = 0; i < valuesGroups.Count; ++i)
        {
            if (valuesGroups[i].name == name)
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

    void InitDataGroup<T>(ref T[] dataGroup)
    {
        List<ValuesGroup> grs = GetValuesGroup(typeof(T).ToString());
        dataGroup = new T[grs.Count];
        for (int i = 0; i < grs.Count; ++i)
        {
            InitDataGroup(ref dataGroup[i], i);
        }
    }

    void InitDataGroup<T>(ref T dataGroup, int index = -1)
    {
        List<ValuesGroup> grs = index == -1 ? GetValuesGroup(typeof(T).ToString()) :
                                              GetValuesGroup(typeof(T).ToString()).GetRange(index, 1);
        if (grs.Count == 0)
        {
            Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        var filds = typeof(T).GetFields();
        for (int i = 0; i < grs.Count; ++i)
        {
            var vals = grs[i].GetVals();
            foreach (var fild in filds)
            {
                if (fild.Name == "Order")
                {
                    object obj1 = dataGroup;
                    fild.SetValue(obj1, i + 1);
                    dataGroup = (T)obj1;
                }

                if (!vals.ContainsKey(fild.Name))
                {
                    //Debug.Log($"В группе {typeof(T)}[{i}] нет поля {fild.Name}");
                    continue;
                }

                object obj = dataGroup;
                if (fild.FieldType == typeof(int))
                {
                    fild.SetValue(obj, (int)vals[fild.Name].Val);
                }
                else
                {
                    fild.SetValue(obj, vals[fild.Name].Val);
                }
                dataGroup = (T)obj;
            }
        }
    }

    void SetInitData<T>(T[] dataGroup)
    {
        for (int i = 0; i < dataGroup.Count(); ++i)
        {
            SetInitData(dataGroup[i], i);
        }
    }

    void SetInitData<T>(T dataGroup, int index = -1)
    {
        List<ValuesGroup> grs = index == -1 ? GetValuesGroup(typeof(T).ToString()) :
                                              GetValuesGroup(typeof(T).ToString()).GetRange(index, 1);
        if (grs.Count == 0)
        {
            //Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        var filds = typeof(T).GetFields();
        for (int i = 0; i < grs.Count; ++i)
        {
            var vals = grs[i].GetVals();
            foreach (var fild in filds)
            {
                if (vals.ContainsKey(fild.Name))
                {
                    if (fild.FieldType == typeof(int))
                    {
                        vals[fild.Name].Val = (int)fild.GetValue(dataGroup);
                    }
                    else if (fild.FieldType == typeof(double))
                    {
                        vals[fild.Name].Val = (double)fild.GetValue(dataGroup);
                    }
                }
            }
        }
    }
}
