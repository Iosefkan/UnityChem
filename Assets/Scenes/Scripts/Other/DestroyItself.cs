using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyItself : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject obj;
    public void Start()
    {
        button.onClick.AddListener(DestroyObject);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(DestroyObject);
    }

    private void DestroyObject()
    {
        Destroy(obj);
    }
}
