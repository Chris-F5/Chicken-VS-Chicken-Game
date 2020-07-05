using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace GameClient.NetworkSynchronisers
{
    abstract class RectObject : NetworkSynchroniser
    {
        [SerializeField]
        Transform attachedTransform;

        [SerializeField]
        BoxCollider2D attachedCollider;

        protected override void HandleEvent(byte _eventId, Packet _packet)
        {
            base.HandleEvent(_eventId, _packet);
            switch (_eventId)
            {
                case EventIds.GameObjects.RectObjects.setPosition:
                    HandleSetPositionEvent(_packet);
                    return;
                case EventIds.GameObjects.RectObjects.setSize:
                    HandleSetSizeEvent(_packet);
                    return;
            }
        }
        protected virtual void HandleSetPositionEvent(Packet _packet)
        {
            Vector2 _newPosition = ReadVector2Event(_packet);
            attachedTransform.position = _newPosition;
        }
        protected virtual void HandleSetSizeEvent(Packet _packet)
        {
            Vector2 _newSize = ReadVector2Event(_packet);
            attachedCollider.size = _newSize;
            attachedCollider.offset = _newSize / 2;
        }
    }
}
