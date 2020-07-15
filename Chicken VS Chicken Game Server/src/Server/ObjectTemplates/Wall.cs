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
        public override void AddComponentsToArray(NetworkObject _objectReference, ref Component[] _componentArray)
        {
            // Components have to be added one by one incase one of them needs to access (through the network object refrence) a previously added one.
            _componentArray = new Component[2];
            _componentArray[0] = 
                new PositionComponent(
                    _objectReference,
                    position
                );
            _componentArray[1] =
                new KenimaticColliderRect(
                    _objectReference,
                    rect
                );
        }
    }
}
