using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameObjects
{
    abstract class DynamicPhysicsObject : RectObject
    {
        public Vector2 velocity;
        public float gravityScale { get; private set; }
        public float drag { get; private set; }
        public float xFriction { get; private set; }
        public float yFriction { get; private set; }
        public bool grounded { get; private set; }
        public DynamicPhysicsObject(byte _typeId, Rect _rect, float _gravityScale = 1, float _drag = 1, float _xFriction = 1, float _yFriction = 1) : base(_typeId, _rect)
        {
            gravityScale = _gravityScale;
            drag = _drag;
            xFriction = _xFriction;
            yFriction = _yFriction;
            velocity = new Vector2(0,0);
        }
        public DynamicPhysicsObject(byte _typeId, Rect _rect, Vector2 _velocity, float _gravityScale = 1, float _drag = 1, float _xFriction = 1, float _yFriction = 1) : base(_typeId, _rect)
        {
            if (_velocity == null)
            {
                throw new System.ArgumentException("Velocity cannot be set to null");
            }
            drag = _drag;
            xFriction = _xFriction;
            yFriction = _yFriction;
            velocity = _velocity;
            gravityScale = _gravityScale;
        }

        public override void Update(Packet _packet)
        {
            Vector2 gravityForce = new Vector2(0, -1) * gravityScale * Constants.GLOBAL_GRAVITY_SCALE * Constants.SECONDS_PER_TICK;
            velocity += gravityForce;

            rect.position += velocity * Constants.SECONDS_PER_TICK;

            grounded = false;
            foreach (KenimaticColliderObject collider in KenimaticColliderObject.allKenimaticColliders)
            {
                CollideWith(collider.rect);
            }

            Drag();

            base.Update(_packet);
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
            else if(_axis == Axis.x)
            {
                velocity.y *= 1 - Constants.SECONDS_PER_TICK * yFriction;
            }
        }
    }
}
