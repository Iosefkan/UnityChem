using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetColorPicker : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private Image image;
    [SerializeField] private Image despAvgImage;
    [SerializeField] private Button button;

    public void Start()
    {
        button.onClick.AddListener(ResetPicker);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(ResetPicker);
    }

    private void ResetPicker()
    {
        popup.SetActive(false);
        despAvgImage.color = Color.black;
        image.sprite = null;
    }
}
