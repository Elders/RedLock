using System;

namespace RedLock
{
    public class LockResult
    {
        public static readonly LockResult Empty = new LockResult(null);

        private LockResult(Mutex mutex)
        {
            Mutex = mutex;
        }

        public bool LockAcquired { get { return Mutex != null; } }

        public Mutex Mutex { get; private set; }

        public static LockResult Create(Mutex mutex)
        {
            if (ReferenceEquals(null, mutex))
            {
                var error = $"Parameter mutex cannot be null. Use {nameof(LockResult)}.{nameof(Empty)} instead.";
                throw new ArgumentNullException(nameof(mutex), error);
            }

            return new LockResult(mutex);
        }
    }
}
