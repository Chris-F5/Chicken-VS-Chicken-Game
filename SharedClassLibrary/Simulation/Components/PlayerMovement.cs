using System;
using System.Collections.Generic;
using SharedClassLibrary.Networking;

namespace SharedClassLibrary.Simulation.Components
{
    public class PlayerMovement : Component
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
        public override void AddStartupEventsToQueue(ref Queue<Event> _queue)
        {
            base.AddStartupEventsToQueue(ref _queue);
            new SetPropertiesEvent(this).AddEventToQueue(ref _queue);
        }
        public override void Update()
        {
            if (acceleration != lastUpdateAcceleration || jumpForce != lastUpdateJumpForce)
            {
                new SetPropertiesEvent(this).AddEventToQueue(ref pendingEvents);

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

        private class SetPropertiesEvent : VirtualEvent
        {
            public float acceleration;
            public float jumpForce;
            public SetPropertiesEvent(PlayerMovement _playerMovement)
            {
                acceleration = _playerMovement.acceleration;
                jumpForce = _playerMovement.jumpForce;
            }
        }
    }
}
