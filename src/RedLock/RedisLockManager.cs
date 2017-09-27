using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RedLock.Logging;
using StackExchange.Redis;

namespace RedLock
{
    public class RedisLockManager : IRedisLockManager
    {
        private static ILog log = LogProvider.GetLogger(nameof(RedisLockManager));

        private const string UnlockScript = @"
            if redis.call(""get"",KEYS[1]) == ARGV[1] then
                return redis.call(""del"",KEYS[1])
            else
                return 0
            end";

        private RedLockOptions options;

        private bool isDisposed = false;

        private ConnectionMultiplexer connection;

        public RedisLockManager(string connectionString) : this(RedLockOptions.Default, connectionString)
        {
        }

        public RedisLockManager(RedLockOptions options, string connectionString)
        {
            if (ReferenceEquals(null, options)) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            this.options = options;

            var configurationOptions = ConfigurationOptions.Parse(connectionString);

            connection = ConnectionMultiplexer.Connect(configurationOptions);
        }

        public LockResult Lock(object resource, TimeSpan ttl)
        {
            return LockAsync(resource, ttl).Result;
        }

        public async Task<LockResult> LockAsync(object resource, TimeSpan ttl)
        {
            return await Retry(() => AcquireLock(resource, ttl), options.LockRetryCount, options.LockRetryDelay);
        }

        public void Unlock(Mutex mutex)
        {
            UnlockAsync(mutex).Wait();
        }

        public async Task UnlockAsync(Mutex lockObject)
        {
            await UnlockInstance(lockObject.Resource, lockObject.Value);
        }

        public bool IsLocked(object resource)
        {
            var key = GetRedisKey(resource);

            if (connection.IsConnected == false)
            {
                log.Warn($"Unreachable endpoint '{connection.ClientName}'.");
                return false;
            }

            try
            {
                return connection.GetDatabase().KeyExists(key, CommandFlags.DemandMaster);
            }
            catch (Exception ex)
            {
                log.WarnException($"Unreachable endpoint '{connection.ClientName}'.", ex);
                return false;
            }
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
                if (connection != null)
                {
                    connection.Dispose();
                }

                isDisposed = true;
            }
        }

        private async Task<LockResult> AcquireLock(object resource, TimeSpan ttl)
        {
            var startTime = DateTime.Now;
            var value = CreateUniqueLockId();

            if (await LockInstance(resource, value, ttl) == false)
            {
                return LockResult.Empty;
            }

            var drift = Convert.ToInt32((ttl.TotalMilliseconds * options.ClockDriveFactor) + 2);
            var validityTime = ttl - (DateTime.Now - startTime) - new TimeSpan(0, 0, 0, 0, drift);

            if (validityTime.TotalMilliseconds > 0)
            {
                return LockResult.Create(new Mutex(resource, value, validityTime));
            }

            await UnlockInstance(resource, value);

            return LockResult.Empty;
        }

        private async Task<bool> LockInstance(object resource, byte[] value, TimeSpan ttl)
        {
            if (connection.IsConnected == false)
            {
                var message = $"Unreachable endpoint '{connection.ClientName}'. Unable to acquire lock on this node.";
                log.Warn(message);

                return false;
            }

            try
            {
                var key = GetRedisKey(resource);
                return await connection.GetDatabase().StringSetAsync(key, value, ttl, When.NotExists, CommandFlags.DemandMaster);
            }
            catch (Exception ex)
            {
                var message = $"Unreachable endpoint '{connection.ClientName}'. Unable to acquire lock on this node.";
                log.WarnException(message, ex);

                return false;
            }
        }

        private async Task UnlockInstance(object resource, byte[] value)
        {
            if (connection.IsConnected == false)
            {
                log.Warn($"Unreachable endpoint '{connection.ClientName}'. Unable to unlock resource '{resource}'.");

                return;
            }

            RedisKey[] key = { GetRedisKey(resource) };
            RedisValue[] values = { value };

            try
            {
                await connection.GetDatabase().ScriptEvaluateAsync(UnlockScript, key, values, CommandFlags.DemandMaster);
            }
            catch (Exception ex)
            {
                log.WarnException($"Unreachable endpoint '{connection.ClientName}'. Unable to unlock resource '{resource}'.", ex);
            }
        }

        private static string GetRedisKey(object resource)
        {
            if (resource is string)
            {
                return resource.ToString();
            }

            var json = JsonConvert.SerializeObject(resource);
            var bytes = GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }

        private static async Task<LockResult> Retry(Func<Task<LockResult>> action, int retryCount, TimeSpan retryDelay)
        {
            var currentRetry = 0;
            var result = LockResult.Empty;

            while (currentRetry++ < retryCount)
            {
                result = await action();
                if (result.LockAcquired) break;

                await Task.Delay(retryDelay);
            }

            return result;
        }

        private static byte[] CreateUniqueLockId()
        {
            return Guid.NewGuid().ToByteArray();
        }
    }
}
