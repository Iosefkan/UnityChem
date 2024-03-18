using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveirt : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Material mt;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        mt.mainTextureOffset = new Vector2(-speed * 100 * Time.time * Time.deltaTime, 0f);
        Vector3 pos = rb.position;
        rb.position += Vector3.forward * -speed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}
