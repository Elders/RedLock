using System;
using System.ComponentModel.DataAnnotations;

namespace Elders.RedLock
{
    public sealed class RedLockOptions
    {
        [Required(AllowEmptyStrings = false)]
        public string ConnectionString { get; set; }

        public ushort LockRetryCount { get; set; } = 1;

        public TimeSpan LockRetryDelay { get; set; } = TimeSpan.FromMilliseconds(10);

        public double ClockDriveFactor { get; set; } = 0.01;
    }
}
