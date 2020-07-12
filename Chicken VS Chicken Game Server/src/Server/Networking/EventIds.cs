﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    static class EventIds
    {
        public const byte EventEnd = 0;
        public const byte StartupEvents = 1;
        public static class PositionComponent
        {
            public const byte SetPosition = 2;
        }
        public static class RectCollider
        {
            public const byte SetSize = 2;
            public const byte SetOfset = 3;
        }

    }
}
