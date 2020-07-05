using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient.NetworkSynchronisers
{
    abstract class DynamicPhysicsRect : RectObject
    {
        protected override void HandleEvent(byte _eventId, Packet _packet)
        {
            base.HandleEvent(_eventId, _packet);

            switch (_eventId)
            {
                case EventIds.GameObjects.RectObjects.DynamicPhysicsRects.setVelocity:
                    HandleSetVelocityEvent(_packet);
                    return;
                case EventIds.GameObjects.RectObjects.DynamicPhysicsRects.setProperties:
                    HandleSetPropertiesEvent(_packet);
                    return;
            }
        }

        private void HandleSetVelocityEvent(Packet _packet)
        {
            // TODO: handle this case
            Vector2 _velocity = ReadVector2Event(_packet);
        }

        private void HandleSetPropertiesEvent(Packet _packet)
        {
            // TODO: handle this case
            ReadFloatArrayEvent(_packet, 4);
        }
    }
}
