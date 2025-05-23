using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("enabled canvas");
    }

    private void OnDisable()
    {
        Debug.Log("disabled canvas");
    }
}
