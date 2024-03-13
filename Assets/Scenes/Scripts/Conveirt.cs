using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveirt : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Material mt;

    private void FixedUpdate()
    {
        mt.mainTextureOffset = new Vector2(Time.time * 10 * Time.deltaTime, 0f);
        Vector3 pos = rb.position;
        rb.position += Vector3.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}
