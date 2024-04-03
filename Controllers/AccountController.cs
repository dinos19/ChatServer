using ChatServer.Models;
using ChatServer.Models.Entity;
using ChatServer.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChatServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AccountHandler AccountHandler;

        public AccountController(ILogger<AccountController> logger, AccountHandler accountHandler)
        {
            _logger = logger;
            AccountHandler = accountHandler;
        }

        [HttpPost("Register")]
        public async Task<Account> Register([FromBody] Account account)
        {
            _logger.LogDebug("Just registered " + JsonConvert.SerializeObject(account));
            return await AccountHandler.RegisterAccount(account);
        }

        [HttpPost("Login")]
        public async Task<bool> Login([FromBody] Account account)
        {
            _logger.LogDebug("Login " + JsonConvert.SerializeObject(account));
            return await AccountHandler.Login(account);
        }
    }
}