using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetController : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();

    private void Update()
    {
        if (IsOwner)
        {
            HandleMovement();
        }
        else
        {
            transform.position = position.Value;
        }
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveSpeed, moveSpeed, moveSpeed) * moveSpeed * Time.deltaTime;
        transform.position += move;

        position.Value = transform.position;
    }
}
