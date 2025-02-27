using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBConnectString : MonoBehaviour
{
    public TextAsset textFile;
    void Start()
    {
        string text = textFile.text;
    }
}
