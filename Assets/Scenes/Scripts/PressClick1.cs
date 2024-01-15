using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackPressClick : MonoBehaviour, ClickedObj
{
    Vector3 pos;
    [SerializeField] private float depth = 0.1f;
    [SerializeField] private float timeBack = 1f;
    private float time= 1f;
    private bool isPressable = true;

    void Start()
    {
        pos = transform.position;
        time = timeBack;
    }

    public void Click(PointerEventData.InputButton ibtn)
    {
        if (!isPressable) return;
        switch (ibtn)
        {
            case PointerEventData.InputButton.Left:
            case PointerEventData.InputButton.Right:
            {
                transform.position += Vector3.forward * depth;
                isPressable = false;
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isPressable) return;

        time -= Time.deltaTime;
        if (time <= 0)
        {
            transform.position = pos;
            time = timeBack;
            isPressable = true;
        }
    }
}
