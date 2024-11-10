using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class UsersWithrole
{
    public int UserId { get; set; }

    public int StorageId { get; set; }

    public int RoleId { get; set; }

    public string Role { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;
}
