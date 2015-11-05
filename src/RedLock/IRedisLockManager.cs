using System;
using System.Threading.Tasks;

namespace RedLock
{
    public interface IRedisLockManager : IDisposable
    {
        bool IsLocked(object resource);

        LockResult Lock(object resource, TimeSpan ttl);

        Task<LockResult> LockAsync(object resource, TimeSpan ttl);

        void Unlock(Mutex mutex);

        Task UnlockAsync(Mutex lockObject);
    }
}