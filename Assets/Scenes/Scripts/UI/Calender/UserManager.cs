using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public TMP_Dropdown roles;
    public TMP_InputField loginInput;
    public TMP_InputField passwordInput;
    public Button addUserButton;
    public Button removeUserButton;
    public SelectableTable userTable;

    private void Awake()
    {
        addUserButton.onClick.AddListener(AddUser);
        removeUserButton.onClick.AddListener(DeleteUser);
        using var ctx = new UsersContext();
        var roleNames = ctx.Roles.Select(r => r.Name).ToList();
        roles.ClearOptions();
        roles.AddOptions(roleNames);
    }

    private void OnDestroy()
    {
        addUserButton.onClick.RemoveListener(AddUser);
        removeUserButton.onClick.RemoveListener(DeleteUser);
    }

    void AddUser()
    {
        var login = loginInput.text;
        var password = HashHelper.GetSha256Hash(passwordInput.text);
        var roleName = roles.captionText.text;
        userTable.AddUser(login, password, roleName);
    }
    
    void DeleteUser()
    {
        userTable.DeleteSelected();
    }

}
