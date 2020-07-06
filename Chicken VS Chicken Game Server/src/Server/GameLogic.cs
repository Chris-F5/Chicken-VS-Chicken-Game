using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    static class GameLogic
    {
        public static uint gameTick { get; private set; } = 0;
        public static void Update()
        {
            if (gameTick == int.MaxValue)
            {
                // TODO: close the program or something
                throw new Exception("Game tick exceeded its max value!");
            }
            gameTick += 1;

            ServerManager.PingClients();

            ThreadManager.UpdateMain();

            NetworkSynchronisers.GameObject.UpdateAll();

            Client.SynchroniseClients();
        }
    }
}
