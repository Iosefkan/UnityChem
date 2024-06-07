using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Netcode;
using UnityEngine;

public class NetController : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();

    public void Start()
    {
        NetworkManager.Singleton.StartHost();
        var v = Dns.GetHostEntry(Dns.GetHostName());
        foreach(var v2 in v.AddressList)
        {
            Debug.Log(v2.ToString());
            if (v2.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                Debug.Log("Internet host " + v2.ToString());
            }
        }
    }

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
