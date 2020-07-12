using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class DynamicPhysicsRect : RectCollider
    {
        private Vector2 velocity;
        public float gravityScale { get; private set; }
        public float drag { get; private set; }
        public float xFriction { get; private set; }
        public float yFriction { get; private set; }
        public bool grounded { get; private set; }

        public DynamicPhysicsRect(NetworkObject _networkObject, Rect _rect, float _gravityScale = 1, float _drag = 1, float _xFriction = 1, float _yFriction = 1) : base(_networkObject, _rect)
        {
            velocity = new Vector2(0, 0);
            gravityScale = _gravityScale;
            drag = _drag;
            xFriction = _xFriction;
            yFriction = _yFriction;
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

            rect.position += velocity * Constants.SECONDS_PER_TICK;

            grounded = false;
            foreach (KenimaticColliderRect colliderRect in KenimaticColliderRect.allKenimaticColliderRects)
            {
                CollideWith(colliderRect.rect);
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

        private void CollideWith(Rect _rect)
        {
            Axis _collisionAxis = rect.CollideWith(_rect);
            if (_collisionAxis == Axis.x)
            {
                velocity.x = 0;
                Friction(Axis.x);
            }
            else if (_collisionAxis == Axis.y)
            {
                if (velocity.y < 0)
                {
                    grounded = true;
                }
                velocity.y = 0;
            }
        }

        private void Drag()
        {
            velocity *= 1 - Constants.SECONDS_PER_TICK * drag;
        }

        private void Friction(Axis _axis)
        {
            if (_axis == Axis.y)
            {
                velocity.x *= 1 - Constants.SECONDS_PER_TICK * xFriction;
            }
            else if (_axis == Axis.x)
            {
                velocity.y *= 1 - Constants.SECONDS_PER_TICK * yFriction;
            }
        }

        private class SetVelocityEvent : SetVector2Event
        {
            public SetVelocityEvent(Vector2 _velocity) :
                base(EventIds.RectCollider.DynamicPhysicsRect.SetVelocity, _velocity)
            { }
        }

        private class SetPropertiesEvent : SetFloatArrayEvent
        {
            public SetPropertiesEvent(DynamicPhysicsRect _object) :
                base
                (
                    EventIds.RectCollider.DynamicPhysicsRect.SetProperties,
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
