using ExploreCalifornia.Models;
using ExploreCalifornia.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ExploreCalifornia
{
    public class ChatHub : Hub
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatHub(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        public override async Task OnConnectedAsync()
        {
            var roomId = await _chatRoomService.CreateRoom(
                Context.ConnectionId);

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                roomId.ToString());

            await Clients.Caller.SendAsync(
                "ReceiveMessage",
                "Explore California",
                DateTimeOffset.UtcNow,
                "Hello! What can we help you with today?");

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string name, string text)
        {
            var roomId = await _chatRoomService.GetRoomForConnectionId(
                Context.ConnectionId);

            var message = new ChatMessage()
            {
                SenderName = name,
                Text = text,
                SentAt = DateTimeOffset.UtcNow
            };

            await _chatRoomService.AddMessage(roomId, message);

            // Broadcast to all clients.
            await Clients.Group(roomId.ToString()).SendAsync(
                "ReceiveMessage",
                message.SenderName,
                message.SentAt,
                message.Text);
        }

        public async Task SetName(string visitorName)
        {
            var roomName = $"Chat with {visitorName} from the web";

            var roomId = await _chatRoomService.GetRoomForConnectionId(
                Context.ConnectionId);

            await _chatRoomService.SetRoomName(roomId, roomName);
        }
    }
}
