using System.Collections.Generic;
using UnityEngine;
using System.Net;
using SharedClassLibrary.Networking;

namespace GameClient
{
    public class NetworkManager : MonoBehaviour
    {
        const string remoteIp = "127.0.0.1";
        const int remotePort = 26950;
        const int localPort = 26950;

        public delegate void PacketHandler(Packet _packet);

        public static NetworkManager instance;

        public static readonly Dictionary<byte, PacketHandler> packetHandlers = new Dictionary<byte, PacketHandler>()
        {
             { (byte)ServerPacketIds.ping, ClientHandle.RecievePing},
             { (byte)ServerPacketIds.welcome, ClientHandle.Welcome },
             { (byte)ServerPacketIds.synchronise, ClientHandle.Synchronise },
        };

        public byte  myId = 255;

        private readonly IPEndPoint remoteEndPoint;

        private Connection connection;

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

        public void SendTcp(Packet _packet)
        {
            connection.SendTcp(_packet);
        }

        public void SendUdp(Packet _packet)
        {
            connection.SendUdp(_packet);
        }

        public void TcpConnectionConfirmed(byte _assignedClientId)
        {
            Debug.Log("Listening For UDP");
            myId = _assignedClientId;
            connection.ConnectUdp(localPort);
        }

        public void ConnectToServer()
        {
            if (connection == null)
            {
                connection = new ClientToServerConnection(remoteEndPoint);
                Debug.Log($"Connecting to {remoteEndPoint}");
                connection.ConnectTcp();
            }
        }
    }
}
