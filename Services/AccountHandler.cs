using ChatServer.Infrastructure.Repositories.BaseAbstractions;
using ChatServer.Models.Entity;

namespace ChatServer.Services
{
    public class AccountHandler
    {
        public AccountHandler(IRepositoryWrapper repos)
        {
            Repos = repos;
        }

        public IRepositoryWrapper Repos { get; set; }

        public async Task<Account> RegisterAccount(Account account) => await Repos.Account.CreateAsync(account);

        public async Task<bool> Login(Account account)
        {
            var res = await Repos.Account.FindByConditionAsync(x => x.Email == account.Email && x.Password == account.Password);
            if (res.Any())
                return true;
            else
                return false;
        }
    }
}