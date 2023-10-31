using Microsoft.AspNetCore.SignalR;
using SignalR.API.DTOs;
using SignalR.API.Services;

namespace SignalR.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatServices _services;

        public ChatHub(ChatServices chatServices)
        {
            _services = chatServices;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "4HGChat");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "4HGChat");
            var user = _services.GetUserByConnectionId(Context.ConnectionId);
            _services.RemoveUserFromList(user); 
            var onlineUsers = _services.GetOnlineUsers();
            await Clients.Groups("4HGChat").SendAsync("OnLineUsers", onlineUsers);
            await DisplayOnlineUsers();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string userName)
        {
            _services.AddUserConnectionId(userName, Context.ConnectionId);
            await DisplayOnlineUsers();
        }

        public async Task ReceivedMessage(MessageDto message)
        {
            await Clients.Groups("4HGChat").SendAsync("NewMessage", message);
        }

        private async Task DisplayOnlineUsers()
        {
            var onlineUsers = _services.GetOnlineUsers();
            await Clients.Groups("4HGChat").SendAsync("OnLineUsers", onlineUsers);
        }
    }
}
