using System;
using System.ComponentModel.DataAnnotations;

namespace Elders.RedLock
{
    public class RedLockOptions
    {
        [Required(AllowEmptyStrings = false)]
        public string ConnectionString { get; set; }

        public int LockRetryCount { get; set; } = 1;

        public TimeSpan LockRetryDelay { get; set; } = TimeSpan.FromMilliseconds(0);

        public double ClockDriveFactor { get; set; } = 0.01;
    }
}
