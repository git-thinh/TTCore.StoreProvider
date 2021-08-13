using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TTCore.StoreProvider.Data;
using TTCore.StoreProvider.Models;

namespace TTCore.StoreProvider.Hubs
{
    public class GroupHub : Hub
    {
        readonly DbMemoryContext _db;
        readonly ILogger<GroupHub> _logger;
        public GroupHub(ILogger<GroupHub> logger, DbMemoryContext db)
        {
            _logger = logger;
            _db = db;
        }

        public Task Send(MessageDto message)
        {
            //if (!_newsStore.GroupExists(newsItem.NewsGroup))
            //    throw new System.Exception("cannot send a news item to a group which does not exist.");

            message.GroupId = _db.Messages.Count() % 3 == 0 ? 1 : 2;
            _db.Add(message);
            return Clients.Group(message.GroupId.ToString()).SendAsync("Send", message);
        }

        public async Task JoinGroup(string groupName)
        {
            //if (!_newsStore.GroupExists(groupName))
            //    throw new System.Exception("cannot join a group which does not exist.");

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("JoinGroup", groupName);

            //var history = _newsStore.GetAllNewsItems(groupName);
            //await Clients.Client(Context.ConnectionId).SendAsync("History", history);
        }

        public async Task LeaveGroup(string groupName)
        {
            //if (!_newsStore.GroupExists(groupName))
            //    throw new System.Exception("cannot leave a group which does not exist.");

            await Clients.Group(groupName).SendAsync("LeaveGroup", groupName);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}