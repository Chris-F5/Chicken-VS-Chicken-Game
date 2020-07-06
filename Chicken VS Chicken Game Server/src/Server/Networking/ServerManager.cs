using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace GameServer
{
    static class ServerManager
    {
        public static int port { get; private set; }
        public static int maxPlayers { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public delegate void PacketHandler(int _fromClient, Packet _packet);

        public static byte currentPingId = 255;
        public static uint pingLoopStartTick;

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

            if (currentPingId == 0)
            {
                pingLoopStartTick = GameLogic.gameTick;
            }

            Client.PingAllClients(currentPingId);
        }
        public static short CalcluatePing(byte _id)
        {
            uint _pingSentTick;
            if (_id > currentPingId)
            {
                _pingSentTick = pingLoopStartTick - 256 + _id;
            }
            else
            {
                _pingSentTick = pingLoopStartTick + _id;
            }

            double _pingDouble = GameLogic.gameTick - _pingSentTick;
            _pingDouble = Math.Floor(_pingDouble);
            if (_pingDouble > short.MaxValue)
            {
                Console.WriteLine("Ping of a client is too high to reprosent as a short. Disconnecting client.");
                // TODO: Disconnect client
                return 0;
            }

            short _ping = (short)_pingDouble;
            return _ping;
        }
    }
}
