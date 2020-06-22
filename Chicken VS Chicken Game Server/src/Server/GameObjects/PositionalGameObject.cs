using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.GameObjects
{
    abstract class PositionalObject : GameObject
    {
        public Vector2 position;
        public PositionalObject(byte _typId, Vector2 _position) : base(_typId)
        {
            if (_position == null) {
                throw new ArgumentException("Position cannot be set to null");
            }
            position = _position;
        }
        public override void SetNewObjectPacketContent(Packet _packet)
        {
            base.SetNewObjectPacketContent(_packet);

            _packet.WriteFloat(position.x);
            _packet.WriteFloat(position.y);
        }

        public override void Update(Packet _packet)
        {
            _packet.WriteShort(objectId);
            _packet.WriteFloat(position.x);
            _packet.WriteFloat(position.y);
        }
    }
}
