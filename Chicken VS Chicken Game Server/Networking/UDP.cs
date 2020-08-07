using System;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Networking;

namespace GameServer
{
    class UDP
    {
        private static UdpClient udpListener;

        public IPEndPoint endPoint;
        private readonly Client client;

        public UDP(Client _client)
        {
            client = _client;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            endPoint = _endPoint;
        }

        public void Send(Packet _packet, bool _writeLength = true)
        {
            try
            {
                if (endPoint != null)
                {
                    if (_writeLength) {
                        _packet.WriteLength();
                    }
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), endPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {endPoint} via UDP: {_ex}");
            }
        }

        public void HandleData(Packet _packetData)
        {
            int _packetLength = _packetData.ReadInt();
            byte[] _packetBytes = _packetData.ReadBytes(_packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    client.HandlePacket(_packet);
                }
            });
        }

        public static void StartListening(int _port)
        {
            udpListener = new UdpClient(_port);
            udpListener.BeginReceive(UDP.RecieveCallback, null);
            Console.WriteLine($"Started listening for UDP on port {_port}");
        }
        public static void RecieveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(RecieveCallback, null);

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
                        byte _claimedClientId = _packet.ReadByte();

                        if (_claimedClientId <= 0 || _claimedClientId > ServerManager.maxPlayers)
                        {
                            // The clients claimed id does not exist. Either they are using a broken client or something went very wrong.
                            Console.WriteLine($"A clients claimed id {_claimedClientId} does not exist. Either they are using a broken client or something went very wrong.");
                        }
                        else if (!ServerManager.clients[_claimedClientId].IsUDPConnected())
                        {
                            Console.WriteLine("New UDP connection established");
                            ServerManager.clients[_claimedClientId].UDPConnect(_clientEndPoint);
                        }
                        else if (ServerManager.clients[_claimedClientId].VerifEndPoint(_clientEndPoint))
                        {
                            ServerManager.clients[_claimedClientId].udp.HandleData(_packet);
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error reciving UDP data: {_ex}");
            }
        }
    }
}