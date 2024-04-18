using ChatServer.Models.Entity;

namespace ChatServer.Models
{
    public class LatestUpdatesResponse
    {
        public List<Account> Accounts { get; set; }
        public List<ChatMessage> Messages { get; set; }
    }
}