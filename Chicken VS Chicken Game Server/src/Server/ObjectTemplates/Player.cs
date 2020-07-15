﻿using System;
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

        public override void AddComponentsToArray(NetworkObject _objectReference, ref Component[] _componentArray)
        {
            _componentArray = new Component[3];
            _componentArray[0] =
                new PositionComponent(
                    _objectReference,
                    position
                    );
            _componentArray[1] =
                new DynamicPhysicsRect(
                    _objectReference,
                    new Rect(
                        new Vector2(-0.5f, 0),
                        new Vector2(1, 1)
                        )
                    );
            _componentArray[2] =
                new PlayerMovement(
                    _objectReference,
                    controller
                    );
        }
    }
}
