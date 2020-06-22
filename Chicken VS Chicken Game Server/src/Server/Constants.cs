using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class Constants
    {
        public const int TICKS_PER_SECOND = 30;
        public const float GLOBAL_GRAVITY_SCALE = 10;

        public const int MS_PER_TICK = 1000 / TICKS_PER_SECOND;
        public const float SECONDS_PER_TICK = 1.0f / TICKS_PER_SECOND;
    }
}
