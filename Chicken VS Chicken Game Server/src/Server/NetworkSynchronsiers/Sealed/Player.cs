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

namespace GameServer.NetworkSynchronisers
{
    sealed class Player : DynamicPhysicsRect
    {
        private float acceleration = 1;
        private float jumpForce = 5;

        private PlayerController controller;

        public Player(PlayerController _controller, Vector2 _position) : base(SynchroniserType.player, new Rect(_position, new Vector2(1,1)),1,1,1,0)
        {
            controller = _controller;
        }

        protected override void AddStartupEventsToPacket(Packet _packet)
        {
            base.AddStartupEventsToPacket(_packet);
            new SetPropertiesEvent(this).AddEventToPacket(_packet);
        }

        public override void Update()
        {
            if (controller.rightKey)
            {
                AddForce(new Vector2(acceleration * Constants.SECONDS_PER_TICK, 0));
            }
            if (controller.leftKey)
            {
                AddForce(new Vector2(-acceleration * Constants.SECONDS_PER_TICK, 0));
            }
            if (controller.upKey && grounded)
            {
                AddForce(new Vector2(0, jumpForce));
            }

            base.Update();
        }

        private class SetPropertiesEvent : SetFloatArrayEvent
        {
            public SetPropertiesEvent(Player _player) :
                base
                (
                    EventIds.GameObjects.RectObjects.DynamicPhysicsRects.Player.setProperties,
                    new float[2]
                    {
                        _player.acceleration,
                        _player.jumpForce
                    }
                )
            { }
        }
    }
}
