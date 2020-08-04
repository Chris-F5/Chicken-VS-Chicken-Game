using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Logging;

namespace SharedClassLibrary.Networking
{
    internal class UDPConnection
    {
        public static bool IsListening { get { return listener != null; } }
        private static UdpClient listener = null;
        private static Connection.UdpSenderIdentifier udpSenderIdentifier;

        private readonly Connection connection;

        private IPEndPoint remoteIpEndPoint;
        private UdpClient socket;

        public UDPConnection(Connection _connection, IPEndPoint _remoteIpEndPoint)
        {
            connection = _connection;
            remoteIpEndPoint = _remoteIpEndPoint;
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);
            socket.Connect(remoteIpEndPoint);
            socket.BeginReceive(RecieveConnectionCallback, null);

            using (Packet _packet = new Packet())
            {
                SendPacket(remoteIpEndPoint, _packet);
            }
        }
        private void RecieveConnectionCallback(IAsyncResult _result)
        {
            // TODO: Check if the source of the end recieve can be trusted.
            byte[] _data = socket.EndReceive(_result, ref remoteIpEndPoint);
            socket.BeginReceive(RecieveConnectionCallback, null);

            if (_data.Length < 4)
            {
                // Something went wrong while transporting the packet. For now, it will be ignored.
                return;
            }
            else
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetLength = _packet.ReadInt();
                    if (_packetLength != _packet.UnreadLength())
                    {
                        Logger.LogWarning("Suggested udp packet length does not equal actual length. Ignoring packet.");
                        return;
                    }
                    connection.HandlePacket(_packet);
                }
            }
        }

        public static void SendPacket(IPEndPoint _endPoint, Packet _packet)
        {
            if (_packet == null)
            {
                throw new ArgumentNullException("_packet can't be null.");
            }
            if (_endPoint == null)
            {
                throw new ArgumentNullException("_endPoint can't be null.");
            }

            _packet.WriteLength();
            listener.BeginSend(_packet.ToArray(), _packet.Length(), _endPoint, null, null);
        }

        public static void StartListening(int _port, Connection.UdpSenderIdentifier _udpSenderIdentifier)
        {
            udpSenderIdentifier = _udpSenderIdentifier;
            if (!IsListening) {
                listener = new UdpClient(_port);
                listener.BeginReceive(ListeningRecieveCallback, null);
                Logger.LogDebug($"Started listening for UDP on port {_port}");
            }
        }

        private static void ListeningRecieveCallback(IAsyncResult _result)
        {
            // Somehow the following line gets the remote end point of the udp connection. No idea how.
            IPEndPoint _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = listener.EndReceive(_result, ref _remoteEndPoint);
            listener.BeginReceive(ListeningRecieveCallback, null);

            if (_data.Length < 4)
            {
                // Something went wrong while transporting the packet. For now, it will be ignored.
                Logger.LogDebug("Something went wrong while transporting a UDP packet. For now, it will be ignored.");
                return;
            }

            Packet _packet = new Packet(_data);
            int _packetLength = _packet.ReadInt();
            if (_data.Length != _packetLength)
            {
                Logger.LogDebug("Suggested udp packet length does not equal actual length. Ignoring packet.");
                return;
            }

            Connection connection = udpSenderIdentifier(_packet, _remoteEndPoint);
            if (connection != null)
            {
                connection.HandlePacket(_packet);
            }
            else
            {
                Logger.LogDebug("An incoming UDP message was rejected based on its endpoint.");
            }
        }
    }
}
