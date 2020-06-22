using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class ServerManager
    {
        public static int port { get; private set; }
        public static int maxPlayers { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;

        private static TcpListener tcpListener;
        private static UdpClient udpListener;

        public static void StartServer(int _port, int _maxPlayers)
        {
            port = _port;
            maxPlayers = _maxPlayers;

            InitClientDict();

            Console.WriteLine("Starting Server...");

            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            udpListener = new UdpClient(port);
            udpListener.BeginReceive(UDPRecieveCallback, null);

            Console.WriteLine($"Server Started on {port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}");

            for (int i = 1; i <= maxPlayers; i++)
            {
                if (!clients[i].IsInUse())
                {
                    clients[i].TCPConnect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: server full");
        }

        private static void UDPRecieveCallback(IAsyncResult _result)
        {
            Console.WriteLine("-");
            try
            {
                Console.WriteLine("Handeling Data");
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
                udpListener.BeginReceive(UDPRecieveCallback, null);

                if (_data.Length < 4)
                {
                    // Something went wrong while transporting the packet. For now, it will be ignored.
                    Console.WriteLine("Something went wrong while transporting the packet. For now, it will be ignored.");
                    return;
                }
                else
                {
                    using (Packet _packet = new Packet(_data))
                    {
                        int _claimedClientId = _packet.ReadInt();

                        if (_claimedClientId <= 0 || _claimedClientId > maxPlayers)
                        {
                            // The clients claimed id does not exist. Either they are using a broken client or something went very wrong.
                            Console.WriteLine($"The clients claimed id {_claimedClientId} does not exist. Either they are using a broken client or something went very wrong.");
                        }
                        else if (!clients[_claimedClientId].IsUDPConnected())
                        {
                            Console.WriteLine("New UDP connection established");
                            clients[_claimedClientId].UDPConnect(_clientEndPoint);
                        }
                        else if (clients[_claimedClientId].VerifEndPoint(_clientEndPoint))
                        {
                            Console.WriteLine($"Client {_claimedClientId} accepting UDP data.");
                            clients[_claimedClientId].udp.HandleData(_packet);
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error reciving UDP data: {_ex}");
            }
            Console.WriteLine("-");
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }
        private static void InitClientDict()
        {
            for (int i = 1; i <= maxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }

            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeRecieved},
                { (int)ClientPackets.udpTestRecieve, ServerHandle.UDPTestRecieved},
                { (int)ClientPackets.buttonDown, ServerHandle.ButtonDown},
                { (int)ClientPackets.buttonUp, ServerHandle.ButtonUp},
            };
        }
    }
}
