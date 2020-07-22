using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    sealed class Wall : ObjectTemplate
    {
        private readonly Vector2 size;
        private readonly Vector2 position;
        public Wall(Vector2 _position, Vector2 _size) : base(NetworkObjectType.wall)
        {
            position = _position;
            size = _size;
        }
        public override void AddComponentsToArray(NetworkObject _objectReference, ref Component[] _componentArray)
        {
            // Components have to be added one by one incase one of them needs to access (through the network object refrence) a previously added one.
            _componentArray = new Component[3];

            _componentArray[0] = 
                new PositionComponent(
                    _objectReference,
                    position);

            _componentArray[1] =
                new RectCollider(
                    _objectReference,
                    size);

            _componentArray[2] =
                new KenimaticCollider(_objectReference);
        }
    }
}
