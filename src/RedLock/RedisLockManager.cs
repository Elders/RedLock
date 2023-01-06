using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Elders.RedLock
{
    public class RedisLockManager : IRedisLockManager, IDisposable
    {
        private ILogger<RedisLockManager> logger;

        private RedLockOptions options;

        private bool isDisposed = false;

        private ConnectionMultiplexer connectionDoNotUse;

        public RedisLockManager(IOptionsMonitor<RedLockOptions> options, ILogger<RedisLockManager> logger)
        {
            this.options = options.CurrentValue;
            this.logger = logger;
        }

        public Task<bool> LockAsync(string resource, TimeSpan ttl)
        {
            return RetryAsync(async () => await AcquireLockAsync(resource, ttl), options.LockRetryCount, options.LockRetryDelay);
        }

        public Task UnlockAsync(string resource)
        {
            return UnlockInstanceAsync(resource);
        }

        public async Task<bool> IsLockedAsync(string resource)
        {
            return await ExecuteAsync(async connection =>
                        await connection.GetDatabase().KeyExistsAsync(resource, CommandFlags.DemandMaster).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                if (connectionDoNotUse != null)
                {
                    connectionDoNotUse.Dispose();
                }

                isDisposed = true;
            }
        }

        private async Task<bool> AcquireLockAsync(string resource, TimeSpan ttl)
        {
            var startTime = DateTime.Now;

            if (await LockInstanceAsync(resource, ttl).ConfigureAwait(false) == false)
            {
                return false;
            }

            var drift = Convert.ToInt32((ttl.TotalMilliseconds * options.ClockDriveFactor) + 2);
            var validityTime = ttl - (DateTime.Now - startTime) - new TimeSpan(0, 0, 0, 0, drift);

            if (validityTime.TotalMilliseconds > 0)
                return true;

            await UnlockInstanceAsync(resource).ConfigureAwait(false);

            logger.LogWarning($"The specified TTL for the resource '{resource}' was too short ({ttl.TotalMilliseconds}ms) and it has been unlocked immediately. Try using longer TTL value.");

            return false;
        }

        private async Task<bool> LockInstanceAsync(string resource, TimeSpan ttl)
        {
            return await ExecuteAsync(async connection =>
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;
                var absExpiration = now.Add(ttl).ToFileTime();

                var prevAbsExpiration = await connection.GetDatabase().StringSetAndGetAsync(resource, absExpiration, ttl, When.Always, CommandFlags.DemandMaster).ConfigureAwait(false);
                if (prevAbsExpiration.HasValue)
                {
                    var nextAbsExpiration = DateTimeOffset.FromFileTime((long)prevAbsExpiration);
                    var newTTl = nextAbsExpiration - now;
                    if (newTTl.TotalMilliseconds > 0)
                    {
                        await connection.GetDatabase().KeyExpireAsync(resource, newTTl, CommandFlags.FireAndForget).ConfigureAwait(false);
                    }
                }

                return prevAbsExpiration.HasValue == false;
            });
        }

        private async Task UnlockInstanceAsync(string resource)
        {
            await ExecuteAsync(async connection =>
                await connection.GetDatabase().KeyDeleteAsync(resource, CommandFlags.DemandMaster | CommandFlags.FireAndForget).ConfigureAwait(false)).ConfigureAwait(false);
        }

        private async Task<bool> RetryAsync(Func<Task<bool>> action, ushort retryCount, TimeSpan retryDelay)
        {
            var currentRetry = 0;
            var result = false;

            while (currentRetry++ <= retryCount)
            {
                try
                {
                    result = await action().ConfigureAwait(false);
                    if (result)
                        return result;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Redlock operation has failed.");
                }

                await Task.Delay(retryDelay).ConfigureAwait(false);
            }

            return result;
        }

        private async Task<T> ExecuteAsync<T>(Func<ConnectionMultiplexer, Task<T>> theLogic)
        {
            if (connectionDoNotUse is null || (connectionDoNotUse.IsConnected == false && connectionDoNotUse.IsConnecting == false))
            {
                try
                {
                    var configurationOptions = ConfigurationOptions.Parse(options.ConnectionString);
                    connectionDoNotUse = await ConnectionMultiplexer.ConnectAsync(configurationOptions);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Unable to establish connection with Redis: {options.ConnectionString}");
                    throw;
                }
            }

            try
            {
                return await theLogic(connectionDoNotUse).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unable to execute Redis query.");

                return default;
            }
        }
    }
}
