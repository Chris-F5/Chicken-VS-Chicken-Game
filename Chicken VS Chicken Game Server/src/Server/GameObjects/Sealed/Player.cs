using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    enum KeyButton
    {
        up = 1,
        down,
        left,
        right
    }
}

namespace GameServer.GameObjects
{
    sealed class Player : DynamicPhysicsObject
    {
        private float acceleration = 1;
        private float jumpForce = 5;

        public bool upKey;
        public bool downKey;
        public bool leftKey;
        public bool rightKey;

        public Player(Vector2 _position) : base((byte)ObjectTypes.player,new Rect(_position, new Vector2(1,1)),1,1,1,0)
        {
        }

        public override void Update(Packet _packet)
        {
            if (rightKey)
            {
                velocity += new Vector2(acceleration * Constants.SECONDS_PER_TICK, 0);
            }
            if (leftKey)
            {
                velocity -= new Vector2(acceleration * Constants.SECONDS_PER_TICK, 0);
            }
            if (upKey && grounded)
            {
                velocity += new Vector2(0, jumpForce);
            }

            base.Update(_packet);
        }
    }
}
