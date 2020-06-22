using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalObject : NetworkObject
{
    [SerializeField]
    Transform attachedTransform;

    public override void HandleNewObjectData(Packet _packet)
    {
        float _xPos = _packet.ReadFloat();
        float _yPos = _packet.ReadFloat();

        attachedTransform.position = new Vector2(_xPos, _yPos);
    }
    public override void HandleObjectUpdate(Packet _packet)
    {
        float _xPos = _packet.ReadFloat();
        float _yPos = _packet.ReadFloat();
        attachedTransform.position = new Vector2(_xPos, _yPos);
    }
}
