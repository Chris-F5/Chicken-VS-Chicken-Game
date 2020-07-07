using GameServer.NetworkSynchronisers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class PlayerController
    {
        public bool rightKey = false;
        public bool leftKey = false;
        public bool upKey = false;

        private Player player;
        public PlayerController()
        {
            player = new Player(this, new Vector2(0, 10));
        }
    }
}
