using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TTCore.StoreProvider.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public override Task OnConnectedAsync()
        {
            var path = this.Context.GetHttpContext().Request.Path;
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("CHAT_MESSAGE", user, message);
        }
    }
}
