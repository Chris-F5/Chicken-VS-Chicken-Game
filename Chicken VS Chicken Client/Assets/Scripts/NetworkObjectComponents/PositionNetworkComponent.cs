using UnityEngine;
using SharedClassLibrary.Networking;

namespace GameClient
{
    class PositionNetworkComponent : NetworkObjectComponent
    {
        [SerializeField]
        Transform attachedTransform;
        public override void HandleEvent(Packet _packet, byte _eventId)
        {
            base.HandleEvent(_packet, _eventId);
            switch (_eventId)
            {
                case EventIds.PositionComponent.SetPosition:
                    attachedTransform.position = ReadVector2Event(_packet);
                    break;
            }
        }
    }
}
