using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    sealed class Wall : NetworkObject
    {
        private Rect initRect;
        private Vector2 initPos;
        public Wall(Vector2 _position, Rect _rect) : base(NetworkObjectType.wall)
        {
            initPos = _position;
            initRect = _rect;
        }
        protected override Component[] InitComponents()
        {
            return new Component[2] 
            {
                new PositionComponent(
                    this,
                    initPos
                    ),
                new KenimaticColliderRect(
                    this,
                    initRect
                    ),
            };
        }
    }
}
