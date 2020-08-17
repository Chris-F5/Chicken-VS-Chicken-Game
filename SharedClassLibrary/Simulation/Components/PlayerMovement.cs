using System;

namespace SharedClassLibrary.Simulation.Components
{
    // TODO: Use decorator pattern to dynamically add multiply behavior types which all effect acceleration, jump force, ect...
    public class PlayerMovement : Component
    {
        private readonly float acceleration;
        private readonly float jumpForce;

        private readonly DynamicPhysicsBehaviour physicsBehaviour;

        public readonly PlayerController controller;

        internal PlayerMovement(Component _component, PlayerController _controller, float _acceleration = 2, float _jumpForce = 8) : base(_component)
        {
            if (_controller == null)
                throw new ArgumentNullException("_controller");

            controller = _controller;
            acceleration = _acceleration;
            jumpForce = _jumpForce;

            physicsBehaviour = GetComponent<DynamicPhysicsBehaviour>();
        }

        protected internal override void Update()
        {
            if (controller != null) {
                if (controller.inputState.rightKey)
                {
                    physicsBehaviour.AddForce(new Vector2(acceleration * Constants.SECONDS_PER_TICK, 0));
                }
                if (controller.inputState.leftKey)
                {
                    physicsBehaviour.AddForce(new Vector2(-acceleration * Constants.SECONDS_PER_TICK, 0));
                }
                if (controller.inputState.upKey && physicsBehaviour.grounded)
                {
                    physicsBehaviour.AddForce(new Vector2(0, jumpForce));
                }
            }

            base.Update();
        }
    }
}
