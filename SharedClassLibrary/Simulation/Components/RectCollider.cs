using System;

namespace SharedClassLibrary.Simulation.Components
{
    public class RectCollider : Collider
    {
        private readonly Vector2 size;
        private readonly Vector2 ofset;

        public readonly PositionComponent positionComponent;

        public RectCollider(Component _nextComponent) : this (_nextComponent, new Vector2(0,0)) { }
        public RectCollider(Component _nextComponent, Vector2 _size) : this(_nextComponent, _size, new Vector2(0, 0)) { }

        public RectCollider(Component _nextComponent, Vector2 _size, Vector2 _ofset) : base(_nextComponent)
        {
            positionComponent = GetComponent<PositionComponent>();

            if (_size == null)
                throw new ArgumentNullException("_size");
            if (_ofset == null)
                throw new ArgumentNullException("_ofset");

            size = _size;
            ofset = _ofset;
        }

        internal override Vector2? CollideWith(Collider _collider)
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
            Vector2 _thisPosition = positionComponent.Position + ofset;
            Vector2 _thatPosition = _rect.positionComponent.Position + _rect.ofset;
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
    }
}
