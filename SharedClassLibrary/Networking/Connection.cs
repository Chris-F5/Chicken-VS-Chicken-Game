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
        private readonly PacketHandler tcpPacketHandler;

        public Connection(IPAddress _remoteIp, int _remotePort, PacketHandler _tcpPacketHandler)
        {
            remoteEndPoint = new IPEndPoint(_remoteIp, _remotePort);
            tcpPacketHandler = _tcpPacketHandler;

            tcpConnection = new TCPConnection(new PacketHandler(HandleTcpPacket));
            tcpConnection.Connect(_remoteIp, _remotePort);
        }

        public Connection(TcpClient _client, PacketHandler _tcpPacketHandler)
        {
            remoteEndPoint = (IPEndPoint)_client.Client.RemoteEndPoint;
            tcpPacketHandler = _tcpPacketHandler;

            tcpConnection = new TCPConnection(new PacketHandler(HandleTcpPacket));
            tcpConnection.AcceptConnection(_client);
        }

        public void HandleTcpPacket(Packet _packet)
        {
            tcpPacketHandler(_packet);
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
