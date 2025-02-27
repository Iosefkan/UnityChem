using UnityEngine;
using CalenderDatabase;
using System;

public class CalenderLoader : MonoBehaviour
{
    void Start()
    {
        try
        {
            var ctx = new CalenderContext();
            Debug.Log("Success");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
