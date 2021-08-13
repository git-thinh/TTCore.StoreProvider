using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCore.StoreProvider.Dtos;

namespace TTCore.StoreProvider.Hubs
{
    // Send messages using Message Pack binary formatter
    public class MsgPackHub : Hub
    {
        public Task Send(MessageDto data)
        {
            return Clients.All.SendAsync("CHANNEL_MESSAGE", data);
        }
    }
}
