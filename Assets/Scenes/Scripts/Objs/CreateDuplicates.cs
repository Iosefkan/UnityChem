using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CreateDuplicates : MonoBehaviour
{
    [SerializeField] private float dilay = 10f;
    [SerializeField] private float destroyDilay = 10f;

    private List<GameObject> extrudatPrefabs = new List<GameObject>();
    int prefCount = 0;

    void Start()
    {
        gameObject.GetChildGameObjects(extrudatPrefabs);
        prefCount = extrudatPrefabs.Count;
        foreach (GameObject go in extrudatPrefabs)
        {
            go.SetActive(false);
        }

        StartCoroutine(CreateObjs());
    }

    IEnumerator CreateObjs()
    {
        while (true)
        {
            CreateObj();
            CreateObj();
            yield return new WaitForSeconds(dilay);
        }
    }

    GameObject CreateObj()
    {
        GameObject newObj = Instantiate(extrudatPrefabs[Random.Range(0, prefCount)], gameObject.transform);
        newObj.GetComponent<Rigidbody>().drag = Random.value;
        newObj.GetComponent<BoxCollider>().material.bounciness = Random.value;
        newObj.SetActive(true);

        Destroy(newObj, destroyDilay);

        return newObj;
    }
}
