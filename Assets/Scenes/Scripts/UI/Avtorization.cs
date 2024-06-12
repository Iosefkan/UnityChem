using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Avtorization : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    [SerializeField] TMP_InputField _loginObj;
    [SerializeField] TMP_InputField _passwordObj;
    [SerializeField] Button _btn;

    [SerializeField] string _login = "login";
    [SerializeField] string _password = "password";

    void Start()
    {
        _btn.onClick.AddListener(TryAvtoriz);
    }

    void TryAvtoriz()
    {
        if (_loginObj.text == _login && _passwordObj.text == _password)
        {
            _panel.SetActive(false);
        }
    }
}
