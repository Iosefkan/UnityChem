using UnityEngine;

public class Conveirt : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 pos;
    [SerializeField] private float speed;
    [SerializeField] private Material mt;
    int counter = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pos = transform.position;

    }

    private void FixedUpdate()
    {
        mt.mainTextureOffset = new Vector2(-speed * 100 * Time.time * Time.fixedDeltaTime, 0f);

        if (counter++ > 10)
        {
            counter = 0;
            transform.position = pos;
        }

        Vector3 rbPos = rb.position;
        rb.position -= speed * Time.fixedDeltaTime * Vector3.forward;
        rb.MovePosition(rbPos);
    }
}
