namespace SharedClassLibrary.Simulation.Components
{
    // TODO: Use decorator pattern to dynamically add multiply physics behavior types which all effect friction, drag, ect...
    public class DynamicPhysicsBehaviour : RollbackComponent
    {
        public readonly float gravityScale;
        public readonly float drag;
        public readonly float xFriction;
        public readonly float yFriction;
        public bool grounded { get; private set; }

        private readonly PositionComponent positionComponent;
        private readonly Collider[] colliders;

        public Vector2 Velocity
        {
            get
            {
                return (state as DynamicPhysicsObjectState).Velocity;
            }
            private set
            {
                (state as DynamicPhysicsObjectState).Velocity = value;
            }
        }

        internal DynamicPhysicsBehaviour(Component _nextComponent, float _gravityScale = 1, float _drag = 1, float _xFriction = 1, float _yFriction = 1)
            : base(_nextComponent, new DynamicPhysicsObjectState( new Vector2(0, 0) ) )
        {
            gravityScale = _gravityScale;
            drag = _drag;
            xFriction = _xFriction;
            yFriction = _yFriction;

            Velocity = new Vector2(0, 0);
            colliders = GetComponents<Collider>().ToArray();
            positionComponent = GetComponent<PositionComponent>();
        }

        protected internal override void Update()
        {
            Vector2 gravityForce = new Vector2(0, -1) * gravityScale * Constants.GLOBAL_GRAVITY_SCALE * Constants.SECONDS_PER_TICK;
            Velocity += gravityForce;

            positionComponent.Position += Velocity * Constants.SECONDS_PER_TICK;

            grounded = false;
            foreach (KenimaticCollider kenimticCollider in KenimaticCollider.allKenimaticColliders)
            {
                foreach (Collider collider in kenimticCollider.colliders)
                {
                    CollideWith(collider);
                }
            }

            ApplyDrag();

            base.Update();
        }

        internal void AddForce(Vector2 _force)
        {
            Velocity += _force;
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

        private void ApplyDrag()
        {
            Velocity *= 1 - Constants.SECONDS_PER_TICK * drag;
        }

        private void CollisionFriction(Vector2 _exitBoundsVector)
        {
            Vector2 exitDirection = Vector2.Normalise(_exitBoundsVector);
            Vector2 velocity = Velocity;
            if (exitDirection.x > 0)
            {
                if (Velocity.x < 0)
                {
                    velocity.x *= exitDirection.x * -1 + 1;
                    velocity.y *= 1 - Constants.SECONDS_PER_TICK * yFriction;
                }
            }
            else if (exitDirection.x < 0)
            {
                if (Velocity.x > 0)
                {
                    velocity.x *= exitDirection.x * -1;
                    velocity.y *= 1 - Constants.SECONDS_PER_TICK * yFriction;
                }
            }
            if (exitDirection.y > 0)
            {
                if (velocity.y < 0)
                {
                    velocity.y *= exitDirection.y * -1 + 1;
                    velocity.x *= 1 - Constants.SECONDS_PER_TICK * xFriction;
                    grounded = true;
                }
            }
            else if (exitDirection.y < 0)
            {
                if (velocity.y > 0)
                {
                    velocity.x *= exitDirection.x * -1;
                    velocity.x *= 1 - Constants.SECONDS_PER_TICK * xFriction;
                }
            }
            Velocity = velocity;
        }
    }

    internal class DynamicPhysicsObjectState : ComponentRollbackState
    {
        public DynamicPhysicsObjectState(Vector2 _velocity)
        {
            velocity = _velocity;
        }

        private Vector2 velocity;
        public Vector2 Velocity 
        {
            get 
            {
                return (activeState as DynamicPhysicsObjectState).velocity;
            } 
            internal set 
            {
                ChangeMade();
                (activeState as DynamicPhysicsObjectState).velocity = value;
            } 
        }

        protected override ComponentRollbackState Clone()
        {
            return new DynamicPhysicsObjectState(velocity);
        }
    }
}
