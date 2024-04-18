using ChatServer.Infrastructure;
using ChatServer.Infrastructure.Repositories;
using ChatServer.Infrastructure.Repositories.BaseAbstractions;
using ChatServer.Models;
using ChatServer.Models.Entity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Debug;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ChatServer.Hubs
{
    public class SyncHub : Hub
    {
        private readonly SharedDB _sharedDB;
        public IRepositoryWrapper Repos { get; set; }

        public SyncHub(SharedDB sharedDB, IRepositoryWrapper repos)
        {
            _sharedDB = sharedDB;
            Repos = repos;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<LatestUpdatesResponse> SyncAsync(LatestUpdates latestUpdates)
        {
            LatestUpdatesResponse res = default(LatestUpdatesResponse);
            //fetch accounts
            var accounts = await Repos.Account.FindByConditionAsync(x => x.AccountId != latestUpdates.CurrentAccount.AccountId && x.CreatedDate >= latestUpdates.Account);
            //fetch messages from this connection.AccountId
            var messages = await Repos.ChatMessage.FindByConditionAsync(x => (x.FromAccountId == latestUpdates.CurrentAccount.AccountId || x.ToAccountId == latestUpdates.CurrentAccount.AccountId) && x.CreatedDate >= latestUpdates.ChatMessage);

            //fetch readUpdates for chatMessages

            res = new LatestUpdatesResponse
            {
                Accounts = accounts,
                Messages = messages
            };
            //await Clients.Caller.SendAsync("GetServerSync", JsonConvert.SerializeObject(res));

            return res;
        }
    }
}