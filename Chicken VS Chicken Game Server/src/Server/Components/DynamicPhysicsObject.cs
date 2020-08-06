using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClassLibrary;
using SharedClassLibrary.Networking;

namespace GameServer
{
    class DynamicPhysicsBehaviour : Component
    {
        private Vector2 velocity;
        public float gravityScale { get; private set; }
        public float drag { get; private set; }
        public float xFriction { get; private set; }
        public float yFriction { get; private set; }
        public bool grounded { get; private set; }

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

        public DynamicPhysicsBehaviour(NetworkObject _networkObject, float _gravityScale = 1, float _drag = 1, float _xFriction = 1, float _yFriction = 1) : base(_networkObject)
        {
            colliders = networkObject.GetComponents<Collider>();
            velocity = new Vector2(0, 0);
            gravityScale = _gravityScale;
            drag = _drag;
            xFriction = _xFriction;
            yFriction = _yFriction;

            positionComponent = networkObject.GetComponent<PositionComponent>();
        }
        public override void AddStartupEventsToPacket(Packet _packet)
        {
            base.AddStartupEventsToPacket(_packet);
            new SetVelocityEvent(velocity).AddEventToPacket(_packet);
            new SetPropertiesEvent(this).AddEventToPacket(_packet);
        }

        public override void Update()
        {
            Vector2 gravityForce = new Vector2(0, -1) * gravityScale * Constants.GLOBAL_GRAVITY_SCALE * Constants.SECONDS_PER_TICK;
            velocity += gravityForce;

            positionComponent.position += velocity * Constants.SECONDS_PER_TICK;

            grounded = false;
            foreach (NetworkObject colliderObject in KenimaticCollider.allKenimaticColliders)
            {
                foreach (Collider collider in colliderObject.GetComponents<Collider>())
                {
                    CollideWith(collider);
                }
            }

            Drag();

            base.Update();
        }

        // Add force function should not be used for changes the client can predict. For example, applying drag.
        public void AddForce(Vector2 _force)
        {
            velocity += _force;
            pendingEvents.Add(new SetVelocityEvent(velocity));
        }

        private void CollideWith(Collider _collider)
        {
            foreach (Collider _thisCollider in colliders)
            {
                Vector2? _exitBounds = _thisCollider.CollideWith(_collider);
                if (_exitBounds != null)
                {
                    positionComponent.position += _exitBounds.Value;
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

        private class SetVelocityEvent : SetVector2Event
        {
            public SetVelocityEvent(Vector2 _velocity) :
                base(EventIds.DynamicPhysicsRect.SetVelocity, _velocity)
            { }
        }

        private class SetPropertiesEvent : SetFloatArrayEvent
        {
            public SetPropertiesEvent(DynamicPhysicsBehaviour _object) :
                base
                (
                    EventIds.DynamicPhysicsRect.SetProperties,
                    new float[4]
                    {
                        _object.gravityScale,
                        _object.drag,
                        _object.xFriction,
                        _object.yFriction,
                    }
                )
            { }
        }
    }
}
