using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    sealed class TestObject : NetworkObject
    {
        public TestObject(Vector2 _position) : base(SynchroniserType.testObject, new Rect(_position,new Vector2(1,1))) {}
    }
}
