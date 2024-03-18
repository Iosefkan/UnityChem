using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDuplicates : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float dilay = 10f;
    [SerializeField] private float destroyDilay = 10f;

    void Start()
    {
        StartCoroutine(CreateObj());
    }

    IEnumerator CreateObj()
    {
        while (true)
        {
            GameObject newObj = Instantiate(prefab, gameObject.transform);
            newObj.SetActive(true);
            Destroy(newObj, destroyDilay);

            yield return new WaitForSeconds(dilay);
        }
    }
}
