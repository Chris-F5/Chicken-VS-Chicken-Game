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

        public static void StartServer(int _port, int _maxPlayers)
        {
            port = _port;
            maxPlayers = _maxPlayers;

            InitClientDict();

            Console.WriteLine("Starting Server...");

            TCP.StartListening(port);
            UDP.StartListening(port);

            Console.WriteLine($"Server Started on {port}.");
        }

        private static void InitClientDict()
        {
            for (int i = 1; i <= maxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }
    }
}
