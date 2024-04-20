using ChatServer.Infrastructure.Repositories.BaseAbstractions;
using ChatServer.Models.Entity;

namespace ChatServer.Infrastructure.Repositories
{
    public class UploadsRepository : RepositoryBase<UploadResult>
    {
        public UploadsRepository(ApiDbContext _dbContext) : base(_dbContext)
        {
        }
    }
}