namespace StorageApp.Model;

public partial class User
{
    public int UserId { get; set; }

    public int StorageId { get; set; }

    public int RoleId { get; set; }

    public string Role { get; set; }

    public string Login { get; set; } 

    public string Password { get; set; }

    public string FirstName { get; set; } 

    public string LastName { get; set; } 

    public string PhoneNum { get; set; }

    public User(  string login, string password, string firstName, string lastName, string phoneNum, int roleId, int storageId)
    {
        StorageId = storageId;
        RoleId = roleId;
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNum = phoneNum;
    }

    public User()
    {
    }
}

