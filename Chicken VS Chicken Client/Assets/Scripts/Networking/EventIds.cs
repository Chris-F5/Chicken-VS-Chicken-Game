using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient
{
    static class EventIds
    {
        public const byte EventEnd = 0;
        public const byte ObjectCreated = 1;
        public static class PositionComponent
        {
            public const byte SetPosition = 2;
        }
        public static class RectCollider
        {
            public const byte SetSize = 2;
            public const byte SetOfset = 3;
            public static class DynamicPhysicsRect
            {
                public const byte SetVelocity = 4;
                public const byte SetProperties = 5;
            }

        }
        public static class PlayerMovement
        {
            public const byte SetProperties = 2;
        }
    }
}
