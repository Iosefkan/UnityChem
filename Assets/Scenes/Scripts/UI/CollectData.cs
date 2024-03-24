using Program;
using System.Collections.Generic;
using UnityEngine;

public class CollectData : MonoBehaviour
{
    private List<ValuesGroup> valuesGroups;
    int counter = 1000;
    public InitData GetInitData()
    {
        valuesGroups = new List<ValuesGroup>(GetComponentsInChildren<ValuesGroup>());

        InitData initData = new InitData(false);
        InitDataArrayGroup(ref initData.cyl);
        InitDataArrayGroup(ref initData.sect);
        InitDataArrayGroup(ref initData.S_K.S);
        initData.S_K.Num_S = initData.S_K.S.Length;
        InitDataGroup(ref initData.data);
        InitDataGroup(ref initData.dop);
        InitDataGroup(ref initData.fluxData);

        return initData;
    }

    public void SetInitData(InitData initData)
    {
        valuesGroups = new List<ValuesGroup>(GetComponentsInChildren<ValuesGroup>());
        List<ValuesGroupArray>  arrays = new List<ValuesGroupArray>(GetComponentsInChildren<ValuesGroupArray>());

        AbjastValuesGroupArray(arrays, initData.cyl.GetType().ToString(), initData.cyl.Length);
        SetInitDataArray(ref initData.cyl);
        AbjastValuesGroupArray(arrays, initData.sect.GetType().ToString(), initData.sect.Length);
        SetInitDataArray(ref initData.sect);
        AbjastValuesGroupArray(arrays, initData.S_K.S.GetType().ToString(), initData.S_K.S.Length);
        SetInitDataArray(ref initData.S_K.S);
        SetInitData(ref initData.data);
        SetInitData(ref initData.dop);
        SetInitData(ref initData.fluxData);
    }

    private void Update()
    {
        if (counter > 0)
        {
            Debug.Log(--counter);
        }
        if (counter == 0)
        {
            InitData init = new InitData();
            SetInitData(init);
            --counter;
        }
    }
    void AbjastValuesGroupArray(List<ValuesGroupArray> arrays, string name, int size)
    {
        foreach (var arr in arrays)
        {
            if (arr.name == name)
            {
                arr.SetSize(size);
            }
        }
    }

    //private void Update()
    //{
    //    InitData id = GetInitData();
    //    Debug.Log(id.cyl[0].Var_T);
    //}

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

    void InitDataArrayGroup<T>(ref T[] dataGroup)
    {
        List<ValuesGroup> grs = GetValuesGroup(typeof(T).ToString());
        if (grs.Count == 0)
        {
            //Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        dataGroup = new T[grs.Count];
        var filds = typeof(T).GetFields();
        for (int i = 0; i < grs.Count; ++i)
        {
            var vals = grs[i].GetVals();
            foreach (var fild in filds)
            {
                if (fild.Name == "Order")
                {
                    object obj1 = dataGroup[i];
                    fild.SetValue(obj1, i + 1);
                    dataGroup[i] = (T)obj1;
                }

                if (!vals.ContainsKey(fild.Name))
                {
                    //Debug.Log($"В группе {typeof(T)}[{i}] нет поля {fild.Name}");
                    continue;
                }

                object obj = dataGroup[i];
                if (fild.FieldType == typeof(int))
                {
                    fild.SetValue(obj, (int)vals[fild.Name].Val);
                }
                else
                {
                    fild.SetValue(obj, vals[fild.Name].Val);
                }
                dataGroup[i] = (T)obj;
            }
        }
    }

    void InitDataGroup<T>(ref T dataGroup)
    {
        List<ValuesGroup> grs = GetValuesGroup(typeof(T).ToString());
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

    void SetInitDataArray<T>(ref T[] dataGroup)
    {
        List<ValuesGroup> grs = GetValuesGroup(typeof(T).ToString());
        if (grs.Count == 0)
        {
            //Debug.Log("На форме не найден тип массива" + typeof(T).ToString());
            return;
        }

        dataGroup = new T[grs.Count];
        var filds = typeof(T).GetFields();
        for (int i = 0; i < grs.Count; ++i)
        {
            var vals = grs[i].GetVals();
            foreach (var fild in filds)
            {
                if (!vals.ContainsKey(fild.Name))
                {
                    vals[fild.Name].Val = (double)fild.GetValue(dataGroup[i]);
                    continue;
                }
            }
        }
    }

    void SetInitData<T>(ref T dataGroup)
    {
        List<ValuesGroup> grs = GetValuesGroup(typeof(T).ToString());
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
                    vals[fild.Name].Val = (double)fild.GetValue(dataGroup);
                }
            }
        }
    }
}
