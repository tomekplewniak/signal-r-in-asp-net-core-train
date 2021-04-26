using ExploreCalifornia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia.Services
{
    public class InMemoryChatRoomService : IChatRoomService
    {
        private readonly Dictionary<Guid, ChatRoom> _roomInfo = new Dictionary<Guid, ChatRoom>();

        public Task<Guid> CreateRoom(string connectionId)
        {
            var id = Guid.NewGuid();
            _roomInfo[id] = new ChatRoom()
            {
                OwnerConnectionId = connectionId
            };

            return Task.FromResult(id);
        }

        public Task<Guid> GetRoomForConnectionId(string connectionId)
        {
            var foundRoom = _roomInfo.FirstOrDefault(
                x => x.Value.OwnerConnectionId == connectionId);

            if (foundRoom.Key == Guid.Empty)
            {
                throw new ArgumentException("Invalid connection ID");
            }

            return Task.FromResult(foundRoom.Key);
        }

        public Task SetRoomName(Guid roomId, string name)
        {
            if (!_roomInfo.ContainsKey(roomId))
            {
                throw new ArgumentException("Invalid room ID");
            }

            _roomInfo[roomId].Name = name;

            return Task.CompletedTask;
        }
    }
}
