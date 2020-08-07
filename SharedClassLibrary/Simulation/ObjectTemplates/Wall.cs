using System;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.ObjectTemplates
{
    sealed class Wall : ObjectTemplate
    {
        private readonly Vector2 size;
        private readonly Vector2 position;
        public Wall(Vector2 _position, Vector2 _size) : base(NetworkObjectTemplateIds.wall)
        {
            if (_position == null)
                throw new ArgumentNullException("_position is null.");
            if (_size == null)
                throw new ArgumentNullException("_size is null.");

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
