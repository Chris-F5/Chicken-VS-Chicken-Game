using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    sealed class Wall : ObjectTemplate
    {
        private Rect rect;
        private Vector2 position;
        public Wall(Vector2 _position, Rect _rect) : base(NetworkObjectType.wall)
        {
            if (_position == null)
                throw new ArgumentNullException("_position is null.");
            if (_rect == null)
                throw new ArgumentNullException("_rect is null.");

            position = _position;
            rect = _rect;
        }
        public override Component[] GenerateCompoentSet(NetworkObject _objectReference)
        {
            return new Component[2] 
            {
                new PositionComponent(
                    _objectReference,
                    position
                    ),
                new KenimaticColliderRect(
                    _objectReference,
                    rect
                    ),
            };
        }
    }
}
