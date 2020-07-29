using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SharedClassLibrary.Networking
{
    internal static class UDP
    {
        public static bool IsListening { get { return listener != null; } }
        private static UdpClient listener = null;
        private static Connection.UdpPacketHandler packetHandler;

        public static void StartListening(int _port, Connection.UdpPacketHandler _udpPacketHandler)
        {
            packetHandler = _udpPacketHandler;
            listener = new UdpClient(_port);
            listener.BeginReceive(RecieveCallback, null);
            Console.WriteLine($"Started listening for UDP on port {_port}");
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
                Console.WriteLine("Something went wrong while transporting a UDP packet. For now, it will be ignored.");
                return;
            }
            else
            {
                using (Packet _packet = new Packet(_data))
                {
                    packetHandler(_packet, _remoteEndPoint);
                }
            }
        }
    }
}
