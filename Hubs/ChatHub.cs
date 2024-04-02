using ChatServer.Infrastructure;
using ChatServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Debug;
using System.Diagnostics;
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
            await Clients.Group(connection.ChatRoom).SendAsync("ReceiveMessage", $"{connection.UserName} has joined {connection.ChatRoom}");
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