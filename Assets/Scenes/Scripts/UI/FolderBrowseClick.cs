using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FolderBrowseClick : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject image;

    public void Start()
    {
        button.onClick.AddListener(Browse);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveListener(Browse);
    }

    public void Browse()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.ShowLoadDialog(OnImageSelected, null, FileBrowser.PickMode.Files, title: "Выберите файл с экструдатом");
    }

    public void OnImageSelected(string[] paths)
    {
        Debug.Log("Selected: " + paths[0]);
        byte[] imageBytes = FileBrowserHelpers.ReadBytesFromFile(paths[0]);
        var rect = (RectTransform)image.transform;
        Texture2D texture = new Texture2D((int)rect.rect.width, (int)rect.rect.height);
        texture.LoadImage(imageBytes);
        var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        image.GetComponent<Image>().sprite = sprite;
        image.GetComponent<AreaSelection>().ResetOriginalTexture();
        image.SetActive(true);
    }
}
