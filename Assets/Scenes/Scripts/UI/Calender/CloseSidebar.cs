using UnityEngine;
using UnityEngine.UI;

public class CloseSidebar : MonoBehaviour
{
    [SerializeField] private GameObject sidebar;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        sidebar.transform.localScale = Vector3.zero;
    }
}
