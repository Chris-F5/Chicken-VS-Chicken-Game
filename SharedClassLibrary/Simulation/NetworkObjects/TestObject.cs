using System;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.ObjectTemplates
{
    public sealed class TestObject : NetworkObject
    {
        public TestObject(Vector2 _position) 
            : base (
                  new PositionComponent(null, _position),
                  (short) NetworkObjectTemplateIds.testObject
                  )
        { }
    }
}
