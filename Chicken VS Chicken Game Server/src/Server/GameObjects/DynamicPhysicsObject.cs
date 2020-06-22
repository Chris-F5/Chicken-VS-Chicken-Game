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
        public DynamicPhysicsObject(byte _typeId, Rect _rect, float _gravityScale = 1) : base(_typeId, _rect)
        {
            gravityScale = _gravityScale;
            velocity = new Vector2(0,0);
        }
        public DynamicPhysicsObject(byte _typeId, Rect _rect, Vector2 _velocity, float _gravityScale = 1) : base(_typeId, _rect)
        {
            if (_velocity == null)
            {
                throw new System.ArgumentException("Velocity cannot be set to null");
            }
            velocity = _velocity;
            gravityScale = _gravityScale;
        }

        public override void Update(Packet _packet)
        {
            Vector2 gravityForce = new Vector2(0, -1) * gravityScale * Constants.GLOBAL_GRAVITY_SCALE * Constants.SECONDS_PER_TICK;
            velocity += gravityForce;

            rect.position += velocity * Constants.SECONDS_PER_TICK;

            Axis _collisionAxis = rect.CollideWith(new Rect(new Vector2(-10, -10), new Vector2(20, 5)));
            if (_collisionAxis == Axis.x)
            {
                velocity.x = 0;
            }
            else if(_collisionAxis == Axis.y)
            {
                velocity.y = 0;
            }

            base.Update(_packet);
        }
    }
}
