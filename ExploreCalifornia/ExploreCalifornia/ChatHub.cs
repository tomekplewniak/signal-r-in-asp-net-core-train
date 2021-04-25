using ExploreCalifornia.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ExploreCalifornia
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string name, string text)
        {
            var message = new ChatMessage()
            {
                SenderName = name,
                Text = text,
                SentAt = DateTimeOffset.UtcNow
            };

            // Broadcast to all clients.
            await Clients.All.SendAsync(
                "ReceiveMessage",
                message.SenderName,
                message.SentAt,
                message.Text);
        }
    }
}
