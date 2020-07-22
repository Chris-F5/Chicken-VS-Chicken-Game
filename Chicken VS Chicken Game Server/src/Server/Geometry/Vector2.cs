using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public struct Vector2
    {
        public float x;
        public float y;
        public float magnitude 
        { 
            get 
            { 
                return (float) Math.Sqrt( Math.Pow(x, 2) + Math.Pow(y, 2) ); 
            }
            set 
            {
                Vector2 normalised = Normalise(this);
                normalised *= value;
                x = normalised.x;
                y = normalised.y;
            }
        }
        public Vector2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public static Vector2 Normalise(Vector2 _vector)
        {
            float _magnitude = _vector.magnitude;
            return _vector * (1 /_magnitude);
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return (v1.x == v2.x && v1.y == v2.y);
        }
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1.x == v2.x && v1.y == v2.y);
        }
    }
}
