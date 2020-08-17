using System;
using SharedClassLibrary.Simulation;

namespace GameServer
{
    static class GameLogic
    {
        public static uint gameTick { get; private set; } = 0;
        public static void Update()
        {
            if (gameTick == uint.MaxValue)
            {
                // TODO: close the program or something
                throw new Exception("Game tick exceeded its max value!");
            }
            gameTick += 1;

            ThreadManager.UpdateMain();

            ServerManager.PingClients();

            NetworkObject.UpdateAll();

            Client.SynchroniseClients();
        }
    }
}
