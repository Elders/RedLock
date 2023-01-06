using System;
using System.ComponentModel.DataAnnotations;

namespace Elders.RedLock
{
    public sealed class RedLockOptions
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(RedLockOptions)}.{nameof(ConnectionString)} is required.")]
        public string ConnectionString { get; set; }

        public ushort LockRetryCount { get; set; } = 1;

        public TimeSpan LockRetryDelay { get; set; } = TimeSpan.FromMilliseconds(10);

        /// <summary>
        /// https://redis.io/docs/manual/patterns/distributed-locks/#safety-arguments
        /// </summary>
        public double ClockDriveFactor { get; set; } = 0.01;
    }
}
