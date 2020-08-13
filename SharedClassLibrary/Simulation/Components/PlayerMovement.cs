using System;

namespace SharedClassLibrary.Simulation.Components
{
    public class PlayerMovement : Component
    {
        private float acceleration;
        private float jumpForce;

        private IPlayerMovementHandler handler;
        private PlayerController controller;
        private DynamicPhysicsBehaviour physicsBehaviour;

        public float Acceleration 
        {
            get { return acceleration; }
            set
            {
                acceleration = value;
                HandleProperties();
            }
        }

        public float JumpForce
        {
            get { return jumpForce; }
            set
            {
                jumpForce = value;
                HandleProperties();
            }
        }

        public PlayerMovement(Component _component, IPlayerMovementHandler _handler, float _acceleration = 2, float _jumpForce = 8) : base(_component)
        {
            if (_handler == null)
                throw new ArgumentNullException("_handler");

            handler = _handler;
            acceleration = _acceleration;
            jumpForce = _jumpForce;

            physicsBehaviour = GetComponent<DynamicPhysicsBehaviour>();
        }
        public void SetPlayerController(PlayerController _controller)
        {
            if (_controller == null)
                throw new ArgumentNullException("_controller");

            controller = _controller;
        }
        internal override void CallStartupHandlers()
        {
            base.CallStartupHandlers();
            HandleProperties();
        }
        internal override void Update()
        {
            if (controller != null) {
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

            base.Update();
        }

        public void HandleProperties()
        {
            handler.SetProperties(acceleration, jumpForce);
        }
    }

    public interface IPlayerMovementHandler
    {
        void SetProperties(float _acceleration, float _jumpForce);
    }
}
