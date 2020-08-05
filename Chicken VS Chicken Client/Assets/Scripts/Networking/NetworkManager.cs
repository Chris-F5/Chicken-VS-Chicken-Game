using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Networking;

namespace GameClient
{
    public class NetworkManager : MonoBehaviour
    {
        const string remoteIp = "127.0.0.1";
        const int remotePort = 25680;
        const int localPort = 25681;

        public delegate void PacketHandler(Packet _packet);

        public static NetworkManager instance;

        public static readonly Dictionary<byte, PacketHandler> packetHandlers = new Dictionary<byte, PacketHandler>()
        {
             { (byte)ServerPacketIds.ping, ClientHandle.RecievePing},
             { (byte)ServerPacketIds.welcome, ClientHandle.Welcome },
             { (byte)ServerPacketIds.synchronise, ClientHandle.Synchronise },
        };

        public byte myId = 255;

        private readonly IPEndPoint remoteEndPoint;

        NetworkManager()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new System.Exception("Only on instance of NetworkManager can exist.");
            }

            remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
        }

        public void TcpConnectionConfirmed(byte _assignedClientId)
        {
            Debug.Log("test 1");
            myId = _assignedClientId;
            UDPConnection.Connect(localPort, remoteEndPoint);
        }

        public void ConnectToServer()
        {
            TCPConnection.Connect(remoteEndPoint);
        }

        public void HandlePacket(Packet _packet)
        {
            byte _typeId = _packet.ReadByte();

            Debug.Log($"Handling Packet id: {_typeId}");

            packetHandlers[_typeId](_packet);
        }
    }
}
