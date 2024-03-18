using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateClick : MonoBehaviour, ClickedObj
{
    [SerializeField] private float rotAng = 10;
    [SerializeField] private float minAng = -10;
    [SerializeField] private float maxAng = 10;

    private float currAng = 0;

    public void Click(PointerEventData.InputButton ibtn)
    {
        switch (ibtn)
        {
            case PointerEventData.InputButton.Left:
            {
                if (currAng + rotAng <= maxAng)
                {
                    transform.Rotate(Vector3.forward, rotAng);
                    currAng += rotAng;
                }
                break;
            }
            case PointerEventData.InputButton.Right:
            {
                if (currAng - rotAng >= minAng)
                {
                    transform.Rotate(Vector3.forward, -rotAng);
                    currAng -= rotAng;
                }
                break;
            }
        }        
    }
}
