using ChatServer.Models.Entity;

namespace ChatServer.Models
{
    public class LoginResponse
    {
        public bool IsLoggedIn { get; set; }
        public List<Account> OnlineAccounts { get; set; }
    }
}