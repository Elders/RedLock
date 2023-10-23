using System;
using System.Threading.Tasks;

namespace Elders.RedLock
{
    public interface IRedisLockManager
    {
        Task<bool> IsLockedAsync(string resource);

        Task<bool> LockAsync(string resource, TimeSpan ttl);

        Task<bool> ExtendLockAsync(string resource, TimeSpan ttl);

        Task UnlockAsync(string resource);
    }
}
