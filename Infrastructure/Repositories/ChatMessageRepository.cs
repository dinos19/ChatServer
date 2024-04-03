using ChatServer.Infrastructure.Repositories.BaseAbstractions;
using ChatServer.Models;

namespace ChatServer.Infrastructure.Repositories
{
    public class ChatMessageRepository : RepositoryBase<ChatMessage>
    {
        public ChatMessageRepository(ApiDbContext _dbContext) : base(_dbContext)
        {
        }
    }
}