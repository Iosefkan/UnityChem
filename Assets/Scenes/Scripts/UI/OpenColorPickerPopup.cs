using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenColorPickerPopup : MonoBehaviour
{
    public GameObject popup;
    [SerializeField] private Button button;
    public ConfirmColorPopup conf;
    [SerializeField] private Image colorPreview;

    public void Start()
    {
        button.onClick.AddListener(PickColor);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(PickColor);
    }

    private void PickColor()
    {
        conf.SetRowPreviewImage(colorPreview);
        popup.SetActive(true);
    }
}
