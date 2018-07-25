using System;
using System.Threading.Tasks;

namespace RedLock
{
    public interface IRedisLockManager : IDisposable
    {
        bool IsLocked(string resource);

        bool Lock(string resource, TimeSpan ttl);

        Task<bool> LockAsync(string resource, TimeSpan ttl);

        void Unlock(string resource);

        Task UnlockAsync(string resource);
    }
}
