using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient.NetworkSynchronisers
{
    sealed class Player : DynamicPhysicsRect
    {
        protected override void HandleEvent(byte _eventId, Packet _packet)
        {
            base.HandleEvent(_eventId, _packet);

            switch (_eventId)
            {
                case EventIds.GameObjects.RectObjects.DynamicPhysicsRects.Player.setProperties:
                    HandleSetPropertiesEvent(_packet);
                    return;
            }
        }

        private void HandleSetPropertiesEvent(Packet _packet)
        {
            ReadFloatArrayEvent(_packet, 2);
        }
    }
}
