using ChatServer.Infrastructure;
using ChatServer.Infrastructure.Repositories;
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
    public class ChatHub : Hub
    {
        private readonly SharedDB _sharedDB;
        private readonly UserConnectionRepository _userConnectionRepo;
        private readonly ChatMessageRepository _chatMessageRepository;

        public ChatHub(SharedDB sharedDB, UserConnectionRepository userConnectionrepo, ChatMessageRepository chatMessageRepository)
        {
            _sharedDB = sharedDB;
            _userConnectionRepo = userConnectionrepo;
            _chatMessageRepository = chatMessageRepository;
        }

        public DebugLoggerProvider DebugLoggerProvider { get; }

        public override async Task OnConnectedAsync()
        {
            List<UserConnection> userConnections = _sharedDB._connections.Values.ToList<UserConnection>();
            //var connection = _sharedDB._connections[Context.ConnectionId];
            _sharedDB._connections.TryGetValue(Context.ConnectionId, out var connection);

            var message = new ChatMessage
            {
                Action = ChatMessageAction.HELLO,
                Body = "",
                FromAccountId = 0,
                ToAccountId = 0,
                Type = ChatMessageType.TEXT
            };

            await Clients.Caller.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(message));

            await base.OnConnectedAsync();
        }

        public async Task<UserConnection> SayHello(Account account)
        {
            var connection = (await _userConnectionRepo.FindByConditionAsync(x => x.AccountId == account.AccountId)).FirstOrDefault();
            if (connection != null)
                await _userConnectionRepo.DeleteAsync(connection);

            return await _userConnectionRepo.CreateAsync(new UserConnection
            {
                AccountId = account.AccountId,
                ChatRoom = account.Name,
                ConnectionId = Context.ConnectionId,
                UserName = account.Name,
                CreatedDate = DateTime.Now,
            });
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = _sharedDB._connections[Context.ConnectionId];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);
            ChatMessage message = new ChatMessage
            {
                Action = ChatMessageAction.ANNOUNCEMENTS,
                Body = $"{connection.UserName} has left {connection.ChatRoom}",
                FromAccountId = 1,
                ToAccountId = connection.AccountId,
                Type = ChatMessageType.TEXT
            };
            await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(message));
            await _userConnectionRepo.DeleteAsync(connection);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<string> JoinChat(UserConnection connection)
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{connection.UserName} has joined");
            Debug.WriteLine($"{connection.UserName} has joined");
            return "joined";
        }

        public async Task JoinSpecificChatRoom(Account account)
        {

            var connectionsRes = await _userConnectionRepo.FindByConditionAsync(x => x.AccountId == account.AccountId);
            var connection = connectionsRes.FirstOrDefault();
            if (connection != null)
            {


                await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
                _sharedDB._connections[Context.ConnectionId] = connection;
                ChatMessage message = new ChatMessage
                {
                    Action = ChatMessageAction.ANNOUNCEMENTS,
                    Body = $"{connection.UserName} has joined {connection.ChatRoom}",
                    FromAccountId = 1,
                    ToAccountId = connection.AccountId,
                    Type = ChatMessageType.TEXT
                };
                await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(message));
            }
        }

        public async Task<ChatMessage> SendMessage(ChatMessage chatMessage)
        {
            ChatMessage res = chatMessage;
            if (chatMessage.Action == ChatMessageAction.NOACTION)
            {
                chatMessage.Status = ChatMessageStatus.SERVER_RECIEVED;
                chatMessage.UpdatedDate = DateTime.Now;
                res = await _chatMessageRepository.CreateAsync(chatMessage);
            }

            var toUser = (await _userConnectionRepo.FindByConditionAsync(x => x.AccountId == chatMessage.ToAccountId)).FirstOrDefault();

            if (toUser != null)
            {
                await Clients.Client(toUser.ConnectionId).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(chatMessage));
            }

            //if (_sharedDB._connections.TryGetValue(Context.ConnectionId, out var connection))
            //{
            //    await Clients.Group(connection.ChatRoom).SendAsync("ReceiveSpecificMessage", connection.UserName, msg);
            //}

            return res;
        }
    }
}