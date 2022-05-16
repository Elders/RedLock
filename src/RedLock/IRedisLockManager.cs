using System;
using System.Threading.Tasks;

namespace Elders.RedLock
{
    public interface IRedisLockManager : IDisposable
    {
        Task<bool> IsLockedAsync(string resource);

        Task<bool> LockAsync(string resource, TimeSpan ttl);

        Task UnlockAsync(string resource);
    }
}
