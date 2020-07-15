using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    sealed class Player : ObjectTemplate
    {
        readonly Vector2 position;
        readonly PlayerController controller;
        public Player(PlayerController _controller, Vector2 _position) : base(NetworkObjectType.player)
        {
            if (_position == null)
                throw new ArgumentNullException("_position is null");

            position = _position;
            controller = _controller;
        }

        public override Component[] GenerateCompoentSet(NetworkObject _objectReference)
        {
            return new Component[3]
            {
                new PositionComponent(
                    _objectReference,
                    position
                    ),
                new DynamicPhysicsRect(
                    _objectReference,
                    new Rect(new Vector2(-0.5f,0), new Vector2(1,1))
                    ),
                new PlayerMovement(
                    _objectReference,
                    controller
                    ),
            };
        }
    }
}
