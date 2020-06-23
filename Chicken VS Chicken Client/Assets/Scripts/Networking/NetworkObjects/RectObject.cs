using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class RectObject : NetworkObject
{
    [SerializeField]
    Transform attachedTransform;

    [SerializeField]
    BoxCollider2D attachedCollider;

    [SerializeField]
    bool changeScale = true;

    public override void HandleNewObjectData(Packet _packet)
    {
        float _xPos = _packet.ReadFloat();
        float _yPos = _packet.ReadFloat();
        float _xSize = _packet.ReadFloat();
        float _ySize = _packet.ReadFloat();

        attachedTransform.position = new Vector2(_xPos, _yPos);
        if (changeScale)
        {
            attachedTransform.localScale = new Vector2(_xSize, _ySize);
        }
        else
        {
            attachedCollider.size = new Vector2(_xSize, _ySize);
            attachedCollider.offset = new Vector2(_xSize / 2, _ySize / 2);
        }
    }
    public override void HandleObjectUpdate(Packet _packet)
    {
        float _xPos = _packet.ReadFloat();
        float _yPos = _packet.ReadFloat();
        attachedTransform.position = new Vector2(_xPos, _yPos);
    }
}
