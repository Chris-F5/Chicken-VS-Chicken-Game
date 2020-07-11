using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    abstract class Collider
    {
        public Vector2 position;
        public void CollideWith(Collider _collider)
        {
            switch (_collider)
            {
                case Rect _rect:
                    this.CollideWith(_rect);
                    return;
                default:
                    throw new Exception($"Collider {_collider.GetType()} cant collide with {this.GetType()}.");
            }
        }
    }
}
