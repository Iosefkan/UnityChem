using UnityEngine;
using UnityEngine.UI;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject xr;
    [SerializeField] private Button startBtn;

    private void Start()
    {
        menu.SetActive(true);
        xr.SetActive(false);

        startBtn.onClick.AddListener(open);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(menu.activeSelf);
            xr.SetActive(xr.activeSelf);
        }
    }

    void open()
    {
        menu.SetActive(false);
        xr.SetActive(true);
    }
}
