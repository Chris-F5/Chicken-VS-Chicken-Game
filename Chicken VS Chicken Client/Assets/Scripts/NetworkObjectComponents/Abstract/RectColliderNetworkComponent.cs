using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameClient
{
    class RectColliderNetworkComponent : NetworkObjectComponent
    {
        [SerializeField]
        BoxCollider2D attachedCollider;
        [SerializeField]
        Vector2 ofset, size;

        public override void HandleEvent(Packet _packet, byte _eventId)
        {
            switch (_eventId)
            {
                case EventIds.RectCollider.SetSize:
                    size = ReadVector2Event(_packet);
                    attachedCollider.size = size;
                    attachedCollider.offset = ofset + (attachedCollider.size / 2);
                    break;
                case EventIds.RectCollider.SetOfset:
                    ofset = ReadVector2Event(_packet);
                    attachedCollider.offset = ofset + (attachedCollider.size / 2);
                    break;
            }
        }
    }
}
