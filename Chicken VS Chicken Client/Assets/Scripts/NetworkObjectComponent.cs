using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    abstract class NetworkObjectComponent : MonoBehaviour
    {
        [SerializeField]
        public readonly byte componentIndex;
        public void HandleSynchronise(Packet _packet)
        {
            while (true)
            {
                byte _eventId = _packet.ReadByte();
                if (_eventId == EventIds.EventEnd)
                {
                    break;
                }
                else
                {
                    HandleEvent(_packet, _eventId);
                }
            }
        }
        public abstract void HandleEvent(Packet _packet, byte EventId);
    }
}
