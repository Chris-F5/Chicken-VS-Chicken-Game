using System;
using System.Collections.Generic;
using SharedClassLibrary.Networking;

namespace GameServer
{
    static class ServerManager
    {
        public static int port { get; private set; }

        public static void StartServer(int _port)
        {
            port = _port;

            Console.WriteLine("Starting Server...");

            TCP.StartListening(port);
            UDP.StartListening(port);

            Console.WriteLine($"Server Started on {port}.");
        }
    }
}
