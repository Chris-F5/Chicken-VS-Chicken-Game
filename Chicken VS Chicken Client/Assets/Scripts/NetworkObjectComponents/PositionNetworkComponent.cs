using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    class PositionNetworkComponent : NetworkObjectComponent
    {
        public override void HandleEvent(Packet _packet, byte _eventId)
        {
            switch (_eventId)
            {
                case EventIds.PositionComponent.SetPosition:
                    transform.position = ReadVector2Event(_packet);
                    break;
            }
        }
    }
}
