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
