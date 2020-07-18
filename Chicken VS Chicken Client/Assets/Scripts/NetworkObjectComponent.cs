using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    abstract class NetworkObjectComponent : MonoBehaviour
    {
        [SerializeField]
        public readonly byte componentIndex;

        public abstract void HandleEvent(Packet _packet, byte EventId);
    }
}
