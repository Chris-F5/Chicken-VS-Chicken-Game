using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Logging;

namespace SharedClassLibrary.Networking
{
    internal static class UDP
    {
        public static bool IsListening { get { return listener != null; } }
        private static UdpClient listener = null;
        private static Connection.UdpSenderIdentifier udpSenderIdentifier;

        public static void StartListening(int _port, Connection.UdpSenderIdentifier _udpSenderIdentifier)
        {
            udpSenderIdentifier = _udpSenderIdentifier;
            if (!IsListening) {
                listener = new UdpClient(_port);
                listener.BeginReceive(RecieveCallback, null);
                Logger.WriteLine($"Started listening for UDP on port {_port}");
            }
        }
        private static void RecieveCallback(IAsyncResult _result)
        {
            // Somehow the following line gets the remote end point of the udp connection. No idea how.
            IPEndPoint _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = listener.EndReceive(_result, ref _remoteEndPoint);
            listener.BeginReceive(RecieveCallback, null);

            if (_data.Length < 4)
            {
                // Something went wrong while transporting the packet. For now, it will be ignored.
                Logger.WriteLine("Something went wrong while transporting a UDP packet. For now, it will be ignored.");
                return;
            }
            else
            {
                Packet _packet = new Packet(_data);
                Connection connection = udpSenderIdentifier(_packet, _remoteEndPoint);
                if (connection != null)
                {
                    connection.HandlePacket(_packet);
                }
                else
                {
                    Logger.WriteLine("An incoming UDP message was rejected based on its endpoint.");
                }
            }
        }
    }
}
