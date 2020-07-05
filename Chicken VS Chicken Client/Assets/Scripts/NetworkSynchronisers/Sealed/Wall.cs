using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace GameClient.NetworkSynchronisers
{
    sealed class Wall : KenimaticColliderObject
    {
        protected override void HandleSetSizeEvent(Packet _packet)
        {
            Vector2 _newSize = ReadVector2Event(_packet);
            transform.localScale = _newSize;
        }
    }
}
