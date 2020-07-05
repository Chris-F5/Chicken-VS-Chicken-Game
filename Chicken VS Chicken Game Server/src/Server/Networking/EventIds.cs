using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public static class EventIds
    {
        public const byte startupEvents = 0;
        public const byte eventsEnd = 1;
        public static class GameObjects
        {
            public static class RectObjects
            {
                public const byte setPosition = 2;
                public const byte setSize = 3;
                public static class DynamicPhysicsRects
                {
                    public const byte setVelocity = 4;
                    public const byte setProperties = 5;
                    public static class Player
                    {
                        public const byte setProperties = 6;
                    }
                }
            }
        }
    }
}
