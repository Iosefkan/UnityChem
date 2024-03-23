using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskValues : MonoBehaviour
{
    [SerializeField] private Value v;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(v.Val);
    }
}
