using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickOnUI : MonoBehaviour
{
    readonly int leftMouseBtn = (int)PointerEventData.InputButton.Left;
    readonly int rightMouseBtn = (int)PointerEventData.InputButton.Right;

    GraphicRaycaster gr;
    PointerEventData ped;

    void Start()
    {
        gr = this.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(leftMouseBtn) || Input.GetMouseButtonDown(rightMouseBtn))
        {
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(ped, results);

            foreach (RaycastResult result in results)
            {
                Button btn = result.gameObject.GetComponent<Button>();
                if (btn) btn.onClick.Invoke();
            }
        }
    }
}
