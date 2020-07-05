using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.NetworkSynchronisers
{
    abstract class RectObject : GameObject
    {
        public Rect rect;
        private bool constantSize;
        public RectObject(SynchroniserType _synchroniseType, Rect _rect, bool _constantSize = true) : base(_synchroniseType)
        {
            if (_rect == null)
            {
                throw new ArgumentException("Rect cannot be set to null");
            }
            constantSize = _constantSize;
            rect = _rect;
        }
        protected override void AddStartupEventsToPacket(Packet _packet)
        {
            base.AddStartupEventsToPacket(_packet);
            new SetPositionEvent(rect.position).AddEventToPacket(_packet);
            new SetSizeEvent(rect.size).AddEventToPacket(_packet);
        }

        public override void Update()
        {
            pendingEvents.Enqueue(new SetPositionEvent(rect.position));
            if (!constantSize) {
                pendingEvents.Enqueue(new SetSizeEvent(rect.size));
            }
        }

        private class SetPositionEvent : SetVector2Event
        {
            public SetPositionEvent(Vector2 _position) : base(EventIds.GameObjects.RectObjects.setPosition, _position) { }
        }
        private class SetSizeEvent : SetVector2Event
        {
            public SetSizeEvent(Vector2 _size) : base(EventIds.GameObjects.RectObjects.setSize, _size) { }
        }
    }
}
