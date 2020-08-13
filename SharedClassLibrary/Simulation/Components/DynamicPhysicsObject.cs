using System;

namespace SharedClassLibrary.Simulation.Components
{
    public class DynamicPhysicsBehaviour : Component
    {
        private Vector2 velocity;
        public float gravityScale { get; private set; }
        public float drag { get; private set; }
        public float xFriction { get; private set; }
        public float yFriction { get; private set; }
        public bool grounded { get; private set; }

        private IDynamicPhysicsObjectHandler handler;
        private readonly PositionComponent positionComponent;
        private readonly Collider[] colliders;

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            private set
            {
                velocity = value;
            }
        }

        public DynamicPhysicsBehaviour(Component _nextComponent, IDynamicPhysicsObjectHandler _handler, float _gravityScale = 1, float _drag = 1, float _xFriction = 1, float _yFriction = 1) : base(_nextComponent)
        {
            if (_handler == null)
                throw new ArgumentNullException("_handler");

            handler = _handler;
            gravityScale = _gravityScale;
            drag = _drag;
            xFriction = _xFriction;
            yFriction = _yFriction;

            velocity = new Vector2(0, 0);
            colliders = GetComponents<Collider>().ToArray();
            positionComponent = GetComponent<PositionComponent>();
        }
        internal override void CallStartupHandlers()
        {
            base.CallStartupHandlers();
            HandleProperties();
            HandleVelocity();
        }

        internal override void Update()
        {
            Vector2 gravityForce = new Vector2(0, -1) * gravityScale * Constants.GLOBAL_GRAVITY_SCALE * Constants.SECONDS_PER_TICK;
            velocity += gravityForce;

            positionComponent.Position += velocity * Constants.SECONDS_PER_TICK;

            grounded = false;
            foreach (KenimaticCollider kenimticCollider in KenimaticCollider.allKenimaticColliders)
            {
                foreach (Collider collider in kenimticCollider.colliders)
                {
                    CollideWith(collider);
                }
            }

            Drag();

            base.Update();
        }

        // Add force function should not be used for changes the client can predict. For example, applying gravity.
        public void AddForce(Vector2 _force)
        {
            velocity += _force;
            HandleVelocity();
        }

        private void CollideWith(Collider _collider)
        {
            foreach (Collider _thisCollider in colliders)
            {
                Vector2? _exitBounds = _thisCollider.CollideWith(_collider);
                if (_exitBounds != null)
                {
                    positionComponent.Position += _exitBounds.Value;
                    CollisionFriction(_exitBounds.Value);
                }
            }
        }

        private void Drag()
        {
            velocity *= 1 - Constants.SECONDS_PER_TICK * drag;
        }

        private void CollisionFriction(Vector2 _exitBoundsVector)
        {
            Vector2 _exitDirection = Vector2.Normalise(_exitBoundsVector);
            if (_exitDirection.x > 0)
            {
                if (velocity.x < 0)
                {
                    velocity.x *= _exitDirection.x * -1 + 1;
                    velocity.y *= 1 - Constants.SECONDS_PER_TICK * yFriction;
                }
            }
            else if (_exitDirection.x < 0)
            {
                if (velocity.x > 0)
                {
                    velocity.x *= _exitDirection.x * -1;
                    velocity.y *= 1 - Constants.SECONDS_PER_TICK * yFriction;
                }
            }
            if (_exitDirection.y > 0)
            {
                if (velocity.y < 0)
                {
                    velocity.y *= _exitDirection.y * -1 + 1;
                    velocity.x *= 1 - Constants.SECONDS_PER_TICK * xFriction;
                    grounded = true;
                }
            }
            else if (_exitDirection.y < 0)
            {
                if (velocity.y > 0)
                {
                    velocity.x *= _exitDirection.x * -1;
                    velocity.x *= 1 - Constants.SECONDS_PER_TICK * xFriction;
                }
            }
        }

        public void HandleProperties()
        {
            handler.SetProperties(gravityScale, drag, xFriction, yFriction);
        }

        public void HandleVelocity()
        {
            handler.SetVelocity(velocity.x, velocity.y);
        }
    }

    public interface IDynamicPhysicsObjectHandler
    {
        void SetProperties(float _gravityScale, float _drag, float _xFriction, float _yFriction);
        /// <summary>
        /// Handles velocity for unexpected events (not including gravity and friction).
        /// </summary>
        void SetVelocity(float _xVelocity, float _yVelocity);
    }
}
