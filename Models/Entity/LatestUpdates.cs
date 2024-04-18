namespace ChatServer.Models.Entity
{
    public class LatestUpdates
    {
        public Account CurrentAccount { get; set; }
        public DateTime Account { get; set; }
        public DateTime ChatMessage { get; set; }
        public DateTime UserConnection { get; set; }
    }
}