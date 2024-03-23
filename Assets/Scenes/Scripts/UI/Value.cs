using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Value : MonoBehaviour
{
    public double Val
    {
        get
        {
            throw new Exception("Метод не реализован");
            return 0;
        }
        set
        {
            throw new Exception("Метод не реализован");
        }
    }

    public int ValI
    {
        get
        {
            throw new Exception("Метод не реализован");
            return 0;
        }
        set
        {
            throw new Exception("Метод не реализован");
        }
    }
}