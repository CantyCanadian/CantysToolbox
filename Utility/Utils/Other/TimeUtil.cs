///====================================================================================================
///
///     TimeUtil by
///     - CantyCanadian
///     
///====================================================================================================

using System;

namespace Canty
{
    public static class TimeUtil
    {
        /// <summary>
        /// Epoch registered when the game starts (0 = Jan 1st 1970)
        /// </summary>
        public static TimeSpan GameStartEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);

        /// <summary>
        /// Gives an int containing how many milliseconds passed since the game started.
        /// </summary>
        /// <returns>How many milliseconds.</returns>
        public static int MillisecondsSinceStartOfSoftware()
        {
            return (DateTime.UtcNow - GameStartEpoch).Minute * 60000 + (DateTime.UtcNow - GameStartEpoch).Second * 1000 + (DateTime.UtcNow - GameStartEpoch).Millisecond;
        }
    }
}