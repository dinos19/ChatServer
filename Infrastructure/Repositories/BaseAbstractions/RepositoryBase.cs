using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatServer.Infrastructure.Repositories.BaseAbstractions
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected ApiDbContext dbContext { get; set; }

        public RepositoryBase(ApiDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<List<T>> FindAllAsync() => await dbContext.Set<T>().AsNoTracking().ToListAsync();

        public async Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression) =>
            await dbContext.Set<T>().Where(expression).AsNoTracking().ToListAsync();

        public async Task<T> CreateAsync(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
    }
}