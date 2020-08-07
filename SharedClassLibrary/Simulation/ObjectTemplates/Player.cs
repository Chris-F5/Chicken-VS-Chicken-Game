using System;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.ObjectTemplates
{
    public sealed class Player : ObjectTemplate
    {
        readonly Vector2 position;
        readonly PlayerController controller;
        public Player(PlayerController _controller, Vector2 _position) : base(NetworkObjectTemplateIds.player)
        {
            if (_position == null)
                throw new ArgumentNullException("_position is null");

            position = _position;
            controller = _controller;
        }

        public override void AddComponentsToArray(NetworkObject _objectReference, ref Component[] _componentArray)
        {
            _componentArray = new Component[4];

            _componentArray[0] =
                new PositionComponent(
                    _objectReference,
                    position);

            _componentArray[1] =
                new RectCollider(
                    _objectReference,
                    new Vector2(1,1));

            _componentArray[2] =
                new DynamicPhysicsBehaviour(_objectReference);

            _componentArray[3] =
                new PlayerMovement(
                    _objectReference,
                    controller);
        }
    }
}
