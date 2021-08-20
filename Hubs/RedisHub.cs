using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TTCore.StoreProvider.Models;

namespace TTCore.StoreProvider.Hubs
{
    public class RedisHub : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.SendAsync("MessageRedis", message);
        }
    }
}
