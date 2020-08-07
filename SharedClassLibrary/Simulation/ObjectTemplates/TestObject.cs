using System;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.ObjectTemplates
{
    public sealed class TestObject : ObjectTemplate
    {
        readonly Vector2 position;
        public TestObject(Vector2 _position) : base(NetworkObjectTemplateIds.testObject)
        {
            if (_position == null)
                throw new ArgumentNullException("_position is null.");

            position = _position;
        }

        public override void AddComponentsToArray(NetworkObject _objectReference, ref Component[] _componentArray)
        {
            _componentArray = new Component[1];
            _componentArray[0] =
                new PositionComponent(
                    _objectReference,
                    position
                );
        }
    }
}
