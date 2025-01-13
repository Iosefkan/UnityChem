using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CreateDuplicates : MonoBehaviour
{
    [SerializeField] private float delay = 10f;
    [SerializeField] private float destroyDelay = 10f;
    public Color extrudatColor = Color.gray;

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

            if (Random.Range(0, 2) == 0)
            {
                StartCoroutine(Wait());
                CreateObj();
            }
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds((float)(1.0 / 1e3));
    }

    GameObject CreateObj()
    {
        GameObject newObj = Instantiate(extrudatPrefabs[Random.Range(0, prefCount)], gameObject.transform);
        newObj.GetComponent<Renderer>().material.color = extrudatColor;
        newObj.GetComponent<Rigidbody>().drag = Random.value;
        newObj.GetComponent<BoxCollider>().material.bounciness = Random.value;
        newObj.SetActive(true);

        Destroy(newObj, destroyDelay);

        return newObj;
    }
}
