using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float rotationSpeed = 300;
    public float moveSpeed = 5;

    void FixedUpdate()
    {
        Rotate();
        Move();
    }

    void Rotate()
    {
        float y = Input.GetAxis("Mouse X");
        float x = -Input.GetAxis("Mouse Y") ;

        transform.eulerAngles += new Vector3(x, y, 0) * Time.deltaTime * rotationSpeed;
    }

    void Move()
    {
        Vector3? moveVector = null;
        if (Input.GetKey(KeyCode.W))
        {
            if (!moveVector.HasValue) moveVector = Vector3.zero;
            moveVector += transform.forward * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (!moveVector.HasValue) moveVector = Vector3.zero;
            moveVector -= transform.forward * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (!moveVector.HasValue) moveVector = Vector3.zero;
            moveVector += transform.right * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!moveVector.HasValue) moveVector = Vector3.zero;
            moveVector -= transform.right * Time.deltaTime * moveSpeed;
        }

        if (moveVector.HasValue)
        {
            Vector3 v = moveVector.Value;
            v.y = 0;
            transform.position += v;
        }
    }
}
