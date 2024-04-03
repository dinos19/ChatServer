using ChatServer.Infrastructure;
using ChatServer.Models;
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

        public ChatHub(SharedDB sharedDB)
        {
            _sharedDB = sharedDB;
        }

        public DebugLoggerProvider DebugLoggerProvider { get; }

        public override async Task OnConnectedAsync()
        {
            List<UserConnection> userConnections = _sharedDB._connections.Values.ToList<UserConnection>();
            //var connection = _sharedDB._connections[Context.ConnectionId];
            _sharedDB._connections.TryGetValue(Context.ConnectionId, out var connection);

            var message = new ChatMessage
            {
                Action = ChatMessageAction.WHOISON,
                Body = JsonConvert.SerializeObject(userConnections),
                FromAccountId = 1,
                ToAccountId = connection.AccountId,
                Type = ChatMessageType.TEXT
            };

            await Clients.Caller.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(message));

            await base.OnConnectedAsync();
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
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<string> JoinChat(UserConnection connection)
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{connection.UserName} has joined");
            Debug.WriteLine($"{connection.UserName} has joined");
            return "joined";
        }

        public async Task JoinSpecificChatRoom(UserConnection connection)
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

        public async Task SendMessage(string msg)
        {
            if (_sharedDB._connections.TryGetValue(Context.ConnectionId, out var connection))
            {
                await Clients.Group(connection.ChatRoom).SendAsync("ReceiveSpecificMessage", connection.UserName, msg);
            }
        }
    }
}