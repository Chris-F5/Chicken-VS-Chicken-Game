using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedGameLogic;

namespace GameServer
{
    class PlayerMovement : Component
    {
        public float acceleration;
        public float jumpForce;

        private PlayerController controller;
        private DynamicPhysicsBehaviour physicsBehaviour;
        private float lastUpdateAcceleration;
        private float lastUpdateJumpForce;

        public PlayerMovement(NetworkObject _networkObject, PlayerController _controller, float _acceleration = 2, float _jumpForce = 8) : base(_networkObject)
        {
            if (_controller == null)
                throw new ArgumentNullException("_controller is null.");

            controller = _controller;
            acceleration = _acceleration;
            jumpForce = _jumpForce;

            physicsBehaviour = _networkObject.GetComponent<DynamicPhysicsBehaviour>();
            lastUpdateAcceleration = acceleration;
            lastUpdateJumpForce = jumpForce;
        }
        public override void AddStartupEventsToPacket(Packet _packet)
        {
            base.AddStartupEventsToPacket(_packet);
            new SetPropertiesEvent(this).AddEventToPacket(_packet);
        }
        public override void Update()
        {
            if (acceleration != lastUpdateAcceleration || jumpForce != lastUpdateJumpForce)
            {
                pendingEvents.Add(new SetPropertiesEvent(this));

                lastUpdateAcceleration = acceleration;
                lastUpdateJumpForce = jumpForce;
            }

            if (controller.rightKey)
            {
                physicsBehaviour.AddForce(new Vector2(acceleration * Constants.SECONDS_PER_TICK, 0));
            }
            if (controller.leftKey)
            {
                physicsBehaviour.AddForce(new Vector2(-acceleration * Constants.SECONDS_PER_TICK, 0));
            }
            if (controller.upKey && physicsBehaviour.grounded)
            {
                physicsBehaviour.AddForce(new Vector2(0, jumpForce));
            }
        }

        private class SetPropertiesEvent : SetFloatArrayEvent
        {
            public SetPropertiesEvent(PlayerMovement _player) :
                base
                (
                    EventIds.PlayerMovement.SetProperties,
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
