using System;

namespace SharedClassLibrary
{
    public class Constants
    {
        // TODO: Setting tick rate to very high number is a good way of finding bugs. Especially thread related.
        public const int TICKS_PER_SECOND = 30;
        public const float GLOBAL_GRAVITY_SCALE = 10;

        public const int MS_PER_TICK = 1000 / TICKS_PER_SECOND;
        public const float SECONDS_PER_TICK = 1.0f / TICKS_PER_SECOND;
    }
}
