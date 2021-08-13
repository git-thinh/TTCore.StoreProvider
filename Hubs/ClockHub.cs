using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TTCore.StoreProvider.Interfaces;
using TTCore.StoreProvider.Models;

namespace TTCore.StoreProvider.Hubs
{
    public class ClockHub : Hub<IClock>
    {
        public async Task SendTimeToClients(DateTime dateTime)
        {
            await Clients.All.ShowTime(dateTime);
        }

        public async Task Send(MessageDto data)
        {
            await Clients.All.Send(data);
        }
    }
}