using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkObject : MonoBehaviour
{
    public short objectId { get; private set; }
    public abstract void HandleNewObjectData(Packet _packet);

    public abstract void HandleObjectUpdate(Packet _packet);
}
