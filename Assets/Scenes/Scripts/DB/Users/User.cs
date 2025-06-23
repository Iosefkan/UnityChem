using System.Collections;
using System.Collections.Generic;

public class User
{
    public long Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; }
}
