using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Logging;

namespace SharedClassLibrary.Networking
{
    public abstract class Connection
    {
        public delegate void NewConnectionHandler(TcpClient _client);
        public delegate Connection UdpSenderIdentifier(Packet _packet, IPEndPoint _ipEndPoint);

        public readonly IPEndPoint remoteEndPoint;
        private readonly TCPConnection tcpConnection;
        private readonly UDPConnection udpConnection;

        protected Connection(IPEndPoint _remoteEndPoint)
        {
            remoteEndPoint = _remoteEndPoint;

            tcpConnection = new TCPConnection(this, remoteEndPoint);
            udpConnection = new UDPConnection(this, remoteEndPoint);
        }

        public Connection(TcpClient _client)
        {
            remoteEndPoint = (IPEndPoint)_client.Client.RemoteEndPoint;

            tcpConnection = new TCPConnection(this, remoteEndPoint);
            tcpConnection.AcceptConnection(_client);
        }

        public abstract void HandlePacket(Packet _packet);

        public void ConnectTcp()
        {
            tcpConnection.Connect();
        }

        public void ConnectUdp(int _localPort)
        {
            udpConnection.Connect(_localPort);
        }

        public void SendUdp(Packet _packet)
        {
            UDPConnection.SendPacket(remoteEndPoint, _packet);
        }

        public void SendTcp(Packet _packet)
        {
            tcpConnection.SendPacket(_packet);
        }

        public static void ListenForNewConnections(int _localPort, NewConnectionHandler _newConnectionHandler)
        {
            if (!TCPConnection.IsListening)
            {
                TCPConnection.StartListening(_localPort, _newConnectionHandler);
            }
        }
        public static void StartListeningForUDP(int _localPort, UdpSenderIdentifier _udpSenderIdentifier)
        {
            if (!UDPConnection.IsListening)
            {
                UDPConnection.StartListening(_localPort, _udpSenderIdentifier);
            }
        }
        public static void SetDataBufferSize(int _dataBufferSize)
        {
            TCPConnection.dataBufferSize = _dataBufferSize;
        }
    }
}
