using ChatServer.Models;
using System.Collections.Concurrent;

namespace ChatServer.Infrastructure
{
    public class SharedDB
    {
        public ConcurrentDictionary<string, UserConnection> _connections { get; set; } = new();
    }
}