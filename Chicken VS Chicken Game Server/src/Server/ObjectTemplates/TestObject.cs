using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    sealed class TestObject : NetworkObject
    {
        Vector2 initPosition;
        public TestObject(Vector2 _position) : base(NetworkObjectType.testObject)
        {
            if (_position == null)
                throw new ArgumentNullException("_position is null.");

            initPosition = _position;
        }
        protected override Component[] InitComponents()
        {
            return new Component[1]
            {
                new PositionComponent(
                    this,
                    initPosition
                    ),
            };
        }
    }
}
