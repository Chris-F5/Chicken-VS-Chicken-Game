using System;
using System.Collections.Generic;
using SharedClassLibrary.Networking;

namespace GameServer
{
    static class ServerManager
    {
        public static int port { get; private set; }
        public static byte maxPlayers { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);

        public static byte currentPingId = 255;

        public static void StartServer(int _port, byte _maxPlayers)
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
            for (byte i = 1; i <= maxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }

        public static void PingClients()
        {
            if (currentPingId == 255)
            {
                currentPingId = 0;
            }
            else
            {
                currentPingId += 1;
            }
            Client.PingAllClients(currentPingId);
        }
        public static byte CalcluatePing(byte _id)
        {
            byte _ping;
            if (_id > currentPingId)
            {
                _ping = (byte)(currentPingId + (256 - _id));
            }
            else
            {
                _ping = (byte)(currentPingId - _id);
            }
            return _ping;
        }
    }
}
