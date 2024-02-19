using Microsoft.AspNetCore.SignalR;

namespace Hospital_FinalP.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("Recieved Message", user, message);
        }
    }
}
