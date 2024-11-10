namespace StorageApi.Model
{
    public partial class UserDTO
    {
        public int UserId { get; set; }
        public int StorageId { get; set; }
        public int RoleId { get; set; }
        public string Login { get; set; } 
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNum { get; set; }
        public string Role { get; set; }
        internal UserDTO(UsersWithrole user)
        {
            UserId = user.UserId;
            StorageId = user.StorageId;
            RoleId = user.RoleId;
            Login = user.Login;
            Password = user.Password;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PhoneNum = user.PhoneNum;
            Role = user.Role;
        }
    }

    public partial class PackageDTO
    {
        public int PackageId { get; set; }

        public decimal Weight { get; set; }

        public string ClientFullname { get; set; } = null!;

        public string ClientMail { get; set; } = null!;

        public string ClientNumber { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime StatusDate { get; set; }

        public int ActionstorageId { get; set; }
    }


}
