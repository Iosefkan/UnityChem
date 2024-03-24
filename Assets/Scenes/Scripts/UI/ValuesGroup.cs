using System.Collections.Generic;
using UnityEngine;

public class ValuesGroup : MonoBehaviour
{
    public string name;

    public Dictionary<string, Value> GetVals()
    {
        Value[] vals = GetComponentsInChildren<Value>();
        Dictionary<string, Value> valsDict = new Dictionary<string, Value>();
        foreach (Value v in vals)
        {
            valsDict.Add(v.name, v);
        }

        return valsDict;
    }
}
