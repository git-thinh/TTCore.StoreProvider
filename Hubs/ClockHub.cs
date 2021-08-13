using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TTCore.StoreProvider.Interfaces;

namespace TTCore.StoreProvider.Hubs
{
    public class ClockHub : Hub<IClock>
    {
        public async Task SendTimeToClients(DateTime dateTime)
        {
            await Clients.All.ShowTime(dateTime);
            //await Clients.All.SendAsync("ReceiveMessage", DateTime.Now.ToString());
        }
    }
}