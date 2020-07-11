using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Rect
    {
        public Vector2 position;
        public Vector2 size;
        
        public Rect(Vector2 _position, Vector2 _size)
        {
            position = _position;
            size = _size;
        }

        public static bool IsColliding(Rect _a, Rect _b)
        {
            return (
                _a.position.x < _b.position.x + _b.size.x &&
                _a.position.x + _a.size.x > _b.position.x &&
                _a.position.y < _b.position.y + _b.size.y &&
                _a.position.y + _a.size.y > _b.position.y
            );
        }

        public Axis CollideWith(Rect _rect)
        {
            if (IsColliding(this, _rect))
            {
                float[] _overlaps = new float[] {
                    _rect.position.x + _rect.size.x - this.position.x,  // Right overlap
                    this.position.x + this.size.x - _rect.position.x,   // Left overlap
                    _rect.position.y + _rect.size.y - this.position.y,  // Up overlap
                    this.position.y + this.size.y - _rect.position.y,   // Down overlap
                };

                float _lowestValue = 0;
                byte _chosenIndex = 4;
                for (byte i = 0; i < 4; i++)
                {
                    if (_lowestValue == 0 || (_overlaps[i] < _lowestValue && _overlaps[i] > 0))
                    {
                        _chosenIndex = i;
                        _lowestValue = _overlaps[i];
                    }
                }
                if (_chosenIndex == 0)
                {
                    position.x += _lowestValue;
                    return Axis.x;
                }
                else if (_chosenIndex == 1)
                {
                    position.x -= _lowestValue;
                    return Axis.x;
                }
                else if (_chosenIndex == 2)
                {
                    position.y += _lowestValue;
                    return Axis.y;
                }
                else if (_chosenIndex == 3)
                {
                    position.y -= _lowestValue;
                    return Axis.y;
                }
                else
                {
                    // This should never run
                    return Axis.none;
                }
            }
            else
            {
                return Axis.none;
            }
        }
    }
}
