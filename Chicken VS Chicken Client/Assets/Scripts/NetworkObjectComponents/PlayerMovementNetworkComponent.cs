﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    class PlayerMovementNetworkComponent : NetworkObjectComponent
    {
        public override void HandleEvent(Packet _packet, byte _eventId)
        {
            switch (_eventId)
            {
                case EventIds.PlayerMovement.SetProperties:
                    // TODO: Calcluate player movement client side to improve predition.
                    _packet.ReadFloat(); // Acceleration
                    _packet.ReadFloat(); // Jump Force
                    break;
            }
        }
    }
}