using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SharedClassLibrary.Networking
{
    public class Connection
    {
        public delegate void PacketHandler(Packet _packet);
        public delegate void NewConnectionHandler(TcpClient _client);
        public delegate void UdpPacketHandler(Packet _packet, IPEndPoint _endpoint);

        public static Connection[] connections;

        public readonly IPEndPoint remoteEndPoint;

        private readonly TCPConnection tcpConnection;
        private readonly PacketHandler packetHandler;

        public Connection(IPAddress _remoteIp, int _remotePort, PacketHandler _packetHandler)
        {
            remoteEndPoint = new IPEndPoint(_remoteIp, _remotePort);
            packetHandler = _packetHandler;

            tcpConnection = new TCPConnection(new PacketHandler(HandlePacket));
            tcpConnection.Connect(_remoteIp, _remotePort);
        }

        public Connection(TcpClient _client, PacketHandler _packetHandler)
        {
            remoteEndPoint = (IPEndPoint)_client.Client.RemoteEndPoint;
            packetHandler = _packetHandler;

            tcpConnection = new TCPConnection(new PacketHandler(HandlePacket));
            tcpConnection.AcceptConnection(_client);
        }

        public void HandlePacket(Packet _packet)
        {
            packetHandler(_packet);
        }

        public static void ListenForNewConnections(NewConnectionHandler _newConnectionHandler, int _localPort)
        {
            if (!TCPConnection.IsListening)
            {
                TCPConnection.StartListening(_localPort, _newConnectionHandler);
            }
        }
        public static void ListenForUDP(int _localPort, UdpPacketHandler _udpPacketHandler)
        {
            if (!UDP.IsListening)
            {
                UDP.StartListening(_localPort, _udpPacketHandler);
            }
        }
    }
}
