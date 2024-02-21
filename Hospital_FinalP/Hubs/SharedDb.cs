using System.Collections.Concurrent;

namespace Hospital_FinalP.Hubs
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connection= new();
        public ConcurrentDictionary<string, UserConnection> connections => _connection;
    }
}
