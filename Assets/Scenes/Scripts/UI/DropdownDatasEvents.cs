using System.Collections.Generic;

public static class DropdownDatasEvents
{
    public delegate void SaveData(object sender, string nameDataGroup, string oldDataName, Dictionary<string, object> dataFields);
    public static SaveData? SaveDataEvent;

    public delegate void AddData(object sender, string nameDataGroup, Dictionary<string, object> dataFields);
    public static AddData? AddDataEvent;

    public delegate void RemoveData(object sender, string nameDataGroup, string dataName);
    public static RemoveData? RemoveDataEvent;
}
