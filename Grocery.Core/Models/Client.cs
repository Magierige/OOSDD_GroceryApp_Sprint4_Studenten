
namespace Grocery.Core.Models
{
    public partial class Client : Model
    {
        public enum Role
        {
            None,
            Admin
        }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Role clientRole { get; set; } 

        public Client(int id, string name, string emailAddress, string password) : base(id, name)
        {
            EmailAddress=emailAddress;
            Password=password;
            clientRole = Role.None;
        }
        public Client(int id, string name, string emailAddress, string password,  Role role) : base(id, name)
        {
            EmailAddress = emailAddress;
            Password = password;
            clientRole = role;
        }
    }
}
