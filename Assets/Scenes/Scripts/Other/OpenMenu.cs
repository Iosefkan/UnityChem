using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    GameObject menu;
    ClickOnObj clickOnObj;
    MoveCharacter moveScript;
    void Start()
    {
        moveScript = GameObject.Find("Main Camera").GetComponent<MoveCharacter>();
        menu = GameObject.Find("UI/Canvas/MenuPanel");
        clickOnObj = GameObject.Find("Settings").GetComponent<ClickOnObj>();


        Button btn = GameObject.Find("UI/Canvas/MenuPanel/ExitButton").GetComponent<Button>();
        if (btn) btn.onClick.AddListener(() => { Application.Quit(); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (menu != null)
            {
                menu.SetActive(!menu.activeSelf);
                if (menu.activeSelf)
                {
                    clickOnObj.enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    moveScript.enabled = false;
                }
                else
                {
                    clickOnObj.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    moveScript.enabled = true;
                }
            }
        }
    }
}
