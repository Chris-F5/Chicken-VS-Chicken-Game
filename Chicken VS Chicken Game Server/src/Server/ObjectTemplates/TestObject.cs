using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    sealed class TestObject : ObjectTemplate
    {
        readonly Vector2 position;
        public TestObject(Vector2 _position) : base(NetworkObjectType.testObject)
        {
            if (_position == null)
                throw new ArgumentNullException("_position is null.");

            position = _position;
        }

        public override Component[] GenerateCompoentSet(NetworkObject _objectReference)
        {
            return new Component[1]
            {
                new PositionComponent(
                    _objectReference,
                    position
                    ),
            };
        }
    }
}
