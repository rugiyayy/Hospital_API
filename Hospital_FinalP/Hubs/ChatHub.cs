using Microsoft.AspNetCore.SignalR;

namespace Hospital_FinalP.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SharedDb _shared;

        public ChatHub(SharedDb shared) => _shared = shared;

        public async Task JoinChat(UserConnection conn)
        {
            await Clients.All
                .SendAsync("RecieveMessage", "admin", $"{conn.UserName} has joined");
        }
        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName: conn.ChatRoom);
            _shared.connections[Context.ConnectionId] = conn;

            await Clients.Group(conn.ChatRoom)
                .SendAsync("JoinSpecificChatRoom", "admin", $"{conn.UserName} has joined {conn.ChatRoom}");
        }

        public async Task SendMessage(string msg)
        {
            if (_shared.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
            {
                await  Clients.Group(conn.ChatRoom).SendAsync("RecievedMessage", conn.UserName, msg);
            }
        }
        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("Recieved Message", user, message);
        //}

    }
}
