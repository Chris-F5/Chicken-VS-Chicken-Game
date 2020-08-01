using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SharedClassLibrary.Networking
{
    public abstract class Connection
    {
        public delegate void NewConnectionHandler(TcpClient _client);
        public delegate Connection UdpSenderIdentifier(Packet _packet, IPEndPoint _ipEndPoint);

        public readonly IPEndPoint remoteEndPoint;
        private readonly TCPConnection tcpConnection; 

        protected Connection(IPAddress _remoteIp, int _remotePort)
        {
            remoteEndPoint = new IPEndPoint(_remoteIp, _remotePort);

            tcpConnection = new TCPConnection(this);
            tcpConnection.Connect(_remoteIp, _remotePort);
        }

        public Connection(TcpClient _client)
        {
            remoteEndPoint = (IPEndPoint)_client.Client.RemoteEndPoint;

            tcpConnection = new TCPConnection(this);
            tcpConnection.AcceptConnection(_client);
        }

        public abstract void HandlePacket(Packet _packet);

        public static void ListenForNewConnections(int _localPort, NewConnectionHandler _newConnectionHandler)
        {
            if (!TCPConnection.IsListening)
            {
                TCPConnection.StartListening(_localPort, _newConnectionHandler);
            }
        }
        public static void StartListeningForUDP(int _localPort, UdpSenderIdentifier _udpSenderIdentifier)
        {
            if (!UDP.IsListening)
            {
                UDP.StartListening(_localPort, _udpSenderIdentifier);
            }
        }
    }
}
