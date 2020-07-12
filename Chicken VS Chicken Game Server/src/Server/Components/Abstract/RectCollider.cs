using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    abstract class RectCollider : Component
    {
        public Rect rect;
        public Vector2 ofset;

        private Vector2 lastUpdateOfset;
        private Vector2 lastUpdateSize;

        public RectCollider(NetworkObject _networkObject, Rect _rect) : base(_networkObject)
        {
            ofset = rect.position;
            lastUpdateOfset = ofset;

            lastUpdateSize = rect.size;

            rect.position += networkObject.GetComponent<PositionComponent>().position;
        }

        public override void AddStartupEventsToPacket(Packet _packet)
        {
            base.AddStartupEventsToPacket(_packet);
            new SetSizeEvent(rect.size).AddEventToPacket(_packet);
            new SetOfsetEvent(ofset).AddEventToPacket(_packet);
        }

        public override void Update()
        {
            if (ofset != lastUpdateOfset) {
                pendingEvents.Add(new SetOfsetEvent(ofset));
                lastUpdateOfset = ofset;
            }
            if (rect.size != lastUpdateSize) {
                pendingEvents.Add(new SetSizeEvent(rect.size));
                lastUpdateSize = rect.size;
            }
        }

        private class SetOfsetEvent : SetVector2Event
        {
            public SetOfsetEvent(Vector2 _ofset) : base(EventIds.RectCollider.SetOfset, _ofset) { }
        }
        private class SetSizeEvent : SetVector2Event
        {
            public SetSizeEvent(Vector2 _size) : base(EventIds.RectCollider.SetOfset, _size) { }
        }
    }
}
