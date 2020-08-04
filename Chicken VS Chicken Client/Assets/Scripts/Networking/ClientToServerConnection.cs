using System.Net;
using System.Net.Sockets;
using UnityEngine;
using SharedClassLibrary.Networking;

namespace GameClient
{
    class ClientToServerConnection : Connection
    {
        public ClientToServerConnection(IPEndPoint _remoteEndPoint) : base(_remoteEndPoint) { }

        public ClientToServerConnection(TcpClient _client) : base(_client) { }

        public override void HandlePacket(Packet _packet)
        {
            byte _typeId = _packet.ReadByte();

            Debug.Log($"Handling Packet id: {_typeId}");

            NetworkManager.packetHandlers[_typeId](_packet);
        }
    }
}
