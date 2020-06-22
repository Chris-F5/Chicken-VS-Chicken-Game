using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.GameObjects
{
    sealed class TestObject : DynamicPhysicsObject
    {
        public TestObject(Vector2 _position) : base((byte)ObjectTypes.testObject, new Rect(_position,new Vector2(1,1)))
        {
            SendNewObjectPacket();
        }
    }
}
