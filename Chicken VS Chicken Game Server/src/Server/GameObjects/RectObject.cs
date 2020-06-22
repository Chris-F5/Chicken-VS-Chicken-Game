using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameObjects
{
    abstract class RectObject : GameObject
    {
        public Rect rect;
        public RectObject(byte _typId, Rect _rect) : base(_typId)
        {
            if (_rect == null)
            {
                throw new ArgumentException("Rect cannot be set to null");
            }
            rect = _rect;
        }
        public override void SetNewObjectPacketContent(Packet _packet)
        {
            base.SetNewObjectPacketContent(_packet);

            _packet.WriteFloat(rect.position.x);
            _packet.WriteFloat(rect.position.y);
            _packet.WriteFloat(rect.size.x);
            _packet.WriteFloat(rect.size.y);
        }

        public override void Update(Packet _packet)
        {
            _packet.WriteShort(objectId);
            _packet.WriteFloat(rect.position.x);
            _packet.WriteFloat(rect.position.y);
        }
    }
}
