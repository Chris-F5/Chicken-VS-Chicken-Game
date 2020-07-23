using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class RectCollider : Collider
    {

        private Vector2 size;
        private Vector2 ofset;
        public readonly PositionComponent positionComponent;

        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                pendingEvents.Add(new SetSizeEvent(size));
            }
        }

        public Vector2 Ofset
        {
            get { return ofset; }
            set
            {
                ofset = value;
                pendingEvents.Add(new SetOfsetEvent(ofset));
            }
        }

        public RectCollider(NetworkObject _networkObject, Vector2 _size) : this(_networkObject, _size, new Vector2(0, 0))
        {
            positionComponent = networkObject.GetComponent<PositionComponent>();
        }

        public RectCollider(NetworkObject _networkObject, Vector2 _size, Vector2 _ofset) : base(_networkObject)
        {
            if (_size == null)
                throw new ArgumentNullException("_size is null.");
            if (_ofset == null)
                throw new ArgumentNullException("_ofset is null.");

            size = _size;
            ofset = _ofset;
        }

        public override void AddStartupEventsToPacket(Packet _packet)
        {
            base.AddStartupEventsToPacket(_packet);
            new SetSizeEvent(size).AddEventToPacket(_packet);
            new SetOfsetEvent(ofset).AddEventToPacket(_packet);
        }

        public override Vector2? CollideWith(Collider _collider)
        {
            switch (_collider)
            {
                case RectCollider _rect:
                    return CollideWithRect(_rect);
                default:
                    throw new ArgumentException(
                        message: "Rect collider cannot collide with this collider.",
                        paramName: nameof(_collider));
            }
        }

        private Vector2? CollideWithRect(RectCollider _rect)
        {
            Vector2 _thisPosition = positionComponent.position + ofset;
            Vector2 _thatPosition = _rect.positionComponent.position + _rect.ofset;
            float[] _overlaps = new float[] {
                    _thatPosition.x + _rect.size.x - _thisPosition.x,  // Right overlap
                    _thisPosition.x + this.size.x - _thatPosition.x,   // Left overlap
                    _thatPosition.y + _rect.size.y - _thisPosition.y,  // Up overlap
                    _thisPosition.y + this.size.y - _thatPosition.y,   // Down overlap
                };

            byte _lowestIndex = 0;
            if (_overlaps[0] <= 0)
            {
                // Not colliding
                return null;
            }
            for (byte i = 1; i < 4; i++)
            {
                if (_overlaps[i] <= 0)
                {
                    // Not colliding
                    return null;
                }
                if (_overlaps[i] < _overlaps[_lowestIndex])
                {
                    _lowestIndex = i;
                }
            }

            switch (_lowestIndex)
            {
                case 0:
                    return new Vector2(_overlaps[_lowestIndex], 0);
                case 1:
                    return new Vector2(-_overlaps[_lowestIndex], 0);
                case 2:
                    return new Vector2(0, _overlaps[_lowestIndex]);
                case 3:
                    return new Vector2(0, -_overlaps[_lowestIndex]);
                default:
                    throw new Exception("_lowest index should be a value from 0 to 3");
            }
        }

        private class SetOfsetEvent : SetVector2Event
        {
            public SetOfsetEvent(Vector2 _ofset) : base(EventIds.Collider.RectCollider.SetOfset, _ofset) { }
        }

        private class SetSizeEvent : SetVector2Event
        {
            public SetSizeEvent(Vector2 _size) : base(EventIds.Collider.RectCollider.SetSize, _size) { }
        }
    }
}
