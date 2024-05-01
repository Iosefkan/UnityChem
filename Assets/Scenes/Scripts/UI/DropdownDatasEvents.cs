using System.Collections.Generic;
using UnityEngine;

public class DropdownDatasEvents : MonoBehaviour
{
    public delegate void SaveData(object sender, string nameDataGroup, string oldDataName, Dictionary<string, object> dataFields);
    public SaveData? SaveDataEvent;

    public delegate void AddData(object sender, string nameDataGroup, Dictionary<string, object> dataFields);
    public AddData? AddDataEvent;

    public delegate void RemoveData(object sender, string nameDataGroup, int index);
    public RemoveData? RemoveDataEvent;
}
