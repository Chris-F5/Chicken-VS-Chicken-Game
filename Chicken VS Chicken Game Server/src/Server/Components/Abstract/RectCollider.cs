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

        public RectCollider(NetworkObject _networkObject, Vector2 _size, Vector2 _ofset) : base(_networkObject)
        {
            ofset = _ofset;
            lastUpdateOfset = ofset;

            lastUpdateSize = _size;

            rect = new Rect(_networkObject.GetComponent<PositionComponent>().position + ofset, _size);
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
