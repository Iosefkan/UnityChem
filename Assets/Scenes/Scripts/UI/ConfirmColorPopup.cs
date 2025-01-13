using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmColorPopup : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private Image avgColor;
    [SerializeField] private Button button;
    [SerializeField] private Image pickerImage;
    private Image imageRowPreview;

    public void SetRowPreviewImage(Image img)
    {
        imageRowPreview = img;
    }

    public void Start()
    {
        button.onClick.AddListener(Confirm);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(Confirm);
    }

    private void Confirm()
    {
        imageRowPreview.color = avgColor.color;
        popup.SetActive(false);
        avgColor.color = Color.black;
        pickerImage.sprite = null;
    }
}
