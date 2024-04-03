using ChatServer.Infrastructure.Repositories.BaseAbstractions;
using ChatServer.Models;

namespace ChatServer.Infrastructure.Repositories
{
    public class UserConnectionRepository : RepositoryBase<UserConnection>
    {
        public UserConnectionRepository(ApiDbContext _dbContext) : base(_dbContext)
        {
        }
    }
}