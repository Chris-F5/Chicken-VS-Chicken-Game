using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    sealed class Player : NetworkObject
    {
        private Vector2 initPosition;
        private PlayerController initController;
        public Player(PlayerController _controller, Vector2 _position) : base(NetworkObjectType.player)
        {
            if (_position == null)
                throw new ArgumentNullException("_position is null");

            initPosition = _position;
            initController = _controller;
        }

        protected override Component[] InitComponents()
        {
            return new Component[3]
            {
                new PositionComponent(
                    this,
                    initPosition
                    ),
                new DynamicPhysicsRect(
                    this,
                    new Rect(new Vector2(-0.5f,0), new Vector2(1,1))
                    ),
                new PlayerMovement(
                    this,
                    initController
                    ),
            };
        }
    }
}
