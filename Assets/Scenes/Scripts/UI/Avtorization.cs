using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Avtorization : MonoBehaviour
{
    [SerializeField] LocalizedString error;
    [SerializeField] LocalizedString roleError;
    [SerializeField] TMP_InputField _loginObj;
    [SerializeField] TMP_InputField _passwordObj;
    [SerializeField] Button _btn;

    [SerializeField] GameObject adminPanel;
    [SerializeField] GameObject calenderInstrPanel;
    [SerializeField] GameObject extrInstrPanel;
    [SerializeField] TMP_Text errorLabel;

    void Awake()
    {
        _btn.onClick.AddListener(TryAuth);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveListener(TryAuth);
    }

    void TryAuth()
    {
        var login = _loginObj.text;
        var password = HashHelper.GetSha256Hash(_passwordObj.text);
        using var ctx = new UsersContext();
        var user = ctx.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
        if (user is null)
        {
            errorLabel.text = error.GetLocalizedString();
            return;
        }
        bool validRole = false;
        switch (user.RoleId)
        {
            case 1:
                validRole = true;
                adminPanel.SetActive(true);
                break;
            case 2:
                validRole = true;
                calenderInstrPanel.SetActive(true);
                break;
            case 3:
                validRole = true;
                extrInstrPanel.SetActive(true);
                break;
            default:
                errorLabel.text = roleError.GetLocalizedString();
                break;
        }

        if (validRole)
        {
            gameObject.SetActive(false);
        }
    }
}
