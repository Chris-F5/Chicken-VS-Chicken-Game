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

        public delegate void PacketHandler(PacketReader _packet);

        public static NetworkManager instance { get; private set; }

        public static readonly Dictionary<byte, PacketHandler> packetHandlers = new Dictionary<byte, PacketHandler>()
        {
             { (byte)ServerPacketIds.welcome, ClientHandle.RecieveWelcome },
             { (byte)ServerPacketIds.shareClientInputs, ClientHandle.RecieveClientInputs },
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
            myId = _assignedClientId;
            UDPConnection.Connect(localPort, remoteEndPoint);
        }

        public void ConnectToServer()
        {
            TCPConnection.Connect(remoteEndPoint);
        }

        public void HandlePacket(PacketReader _packet)
        {
            byte _typeId = _packet.ReadByte();

            Debug.Log($"Handling Packet id: {_typeId}");

            packetHandlers[_typeId](_packet);
        }

        public void SendTcp(PacketWriter _packet)
        {
            TCPConnection.SendData(_packet.GetGeneratedBytes());
        }
        public void SendUdp(PacketWriter _packet)
        {

        }
    }
}
