using UnityEngine;
using System;

[Serializable]
public class Value : MonoBehaviour
{
    public string name = string.Empty;
    public object val;

    public virtual object Val
    {
        get
        {
            return val;
        }
        set
        {
            val = value;
        }
    }

    public virtual object DefaultVal 
    {
        get
        {
            return 0;
        }
    }
}
