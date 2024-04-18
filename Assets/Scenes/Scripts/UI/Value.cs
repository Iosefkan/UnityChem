using UnityEngine;
using System;

[Serializable]
public class Value : MonoBehaviour
{
    public string name;

    public virtual object Val
    {
        get
        {
            throw new Exception("Метод не реализован");
        }
        set
        {
            throw new Exception("Метод не реализован");
        }
    }
}
