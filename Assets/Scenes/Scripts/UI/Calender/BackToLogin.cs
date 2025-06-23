using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToLogin : MonoBehaviour
{
    [SerializeField] GameObject currentPanel;
    [SerializeField] GameObject authorizePanel;
    [SerializeField] Button button;

    private void Awake()
    {
        button.onClick.AddListener(GoBack);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(GoBack);
    }

    private void GoBack()
    {
        authorizePanel.SetActive(true);
        currentPanel.SetActive(false);
    }
}
