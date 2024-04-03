using ChatServer.Infrastructure.Repositories.BaseAbstractions;
using ChatServer.Models.Entity;

namespace ChatServer.Infrastructure.Repositories
{
    public class AccountRepository : RepositoryBase<Account>
    {
        public AccountRepository(ApiDbContext _dbContext) : base(_dbContext)
        {
        }
    }
}