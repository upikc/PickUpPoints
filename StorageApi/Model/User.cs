using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class User
{
    public int UserId { get; set; }

    public int StorageId { get; set; }

    public int RoleId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNum { get; set; } = null!;

    public int Enable { get; set; }

    public virtual ICollection<PkgOperation> PkgOperations { get; set; } = new List<PkgOperation>();

    public virtual UserRole Role { get; set; } = null!;

    public virtual Storage Storage { get; set; } = null!;
    
    public User(string login, string password, string firstName, string lastName, string phoneNum, int roleId, int storageId)
    {
        StorageId = storageId;
        RoleId = roleId;
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNum = phoneNum;
    }


}
