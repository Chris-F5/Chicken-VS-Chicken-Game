using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    class DynamicPhysicsRectNetworkComponent : RectColliderNetworkComponent
    {
        [SerializeField]
        Rigidbody2D attachedRigidbody;

        public override void HandleEvent(Packet _packet, byte _eventId)
        {
            base.HandleEvent(_packet, _eventId);

            switch (_eventId)
            {
                case EventIds.RectCollider.DynamicPhysicsRect.SetVelocity:
                    attachedRigidbody.velocity = ReadVector2Event(_packet);
                    break;
                case EventIds.RectCollider.DynamicPhysicsRect.SetProperties:
                    attachedRigidbody.gravityScale = _packet.ReadFloat();
                    attachedRigidbody.drag = _packet.ReadFloat();
                    // TODO: calcluate x and y friction client side to improve prediction.
                    _packet.ReadFloat(); // X Friction
                    _packet.ReadFloat(); // Y Friction
                    break;
            }
        }
    }
}
