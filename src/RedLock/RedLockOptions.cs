using System;

namespace Elders.RedLock
{
    public class RedLockOptions
    {
        private static readonly RedLockOptions defaults = new RedLockOptions
        {
            LockRetryCount = 1,
            LockRetryDelay = TimeSpan.FromMilliseconds(0),
            ClockDriveFactor = 0.01
        };

        public int LockRetryCount { get; set; }

        public TimeSpan LockRetryDelay { get; set; }

        public double ClockDriveFactor { get; set; }

        public static RedLockOptions Default { get { return defaults; } }
    }
}
