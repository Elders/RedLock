using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedLock
{
    public class RedisLockManager : IRedisLockManager
    {
        private const string UnlockScript = @"
            if redis.call(""get"",KEYS[1]) == ARGV[1] then
                return redis.call(""del"",KEYS[1])
            else
                return 0
            end";

        private RedLockOptions options;

        private bool isDisposed = false;

        private IList<ConnectionMultiplexer> connections;

        private int Quorum { get { return (connections.Count / 2) + 1; } }

        public RedisLockManager(IEnumerable<IPEndPoint> redisEndpoints) : this(RedLockOptions.Default, redisEndpoints)
        {
        }

        public RedisLockManager(RedLockOptions options, IEnumerable<IPEndPoint> redisEndpoints)
        {
            if (ReferenceEquals(null, options)) throw new ArgumentNullException(nameof(options));
            if (ReferenceEquals(null, redisEndpoints)) throw new ArgumentNullException(nameof(redisEndpoints));
            if (!redisEndpoints.Any()) throw new ArgumentException(nameof(redisEndpoints), "No Redis endpoints provided.");

            connections = new List<ConnectionMultiplexer>();
            this.options = options;

            foreach (var endpoint in redisEndpoints)
            {
                // TODO: ConnectionMultiplexer.Connect might throw an exception.
                connections.Add(ConnectionMultiplexer.Connect(endpoint.ToString()));
            }
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
            foreach (var connection in connections)
            {
                await UnlockInstance(connection, lockObject.Resource, lockObject.Value);
            }
        }

        public bool IsLocked(object resource)
        {
            var key = GetRedisKey(resource);
            var results = new List<bool>();

            foreach (var connection in connections)
            {
                results.Add(connection.GetDatabase().KeyExists(key));
            }

            return results.Count(x => x == true) >= Quorum;
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
                if (connections != null)
                {
                    foreach (var connection in connections)
                    {
                        connection.Dispose();
                    }

                    connections = null;
                }

                isDisposed = true;
            }
        }

        private async Task<LockResult> AcquireLock(object resource, TimeSpan ttl)
        {
            try
            {
                var n = 0;
                var startTime = DateTime.Now;
                var value = CreateUniqueLockId();

                foreach (var connection in connections)
                {
                    if (await LockInstance(connection, resource, value, ttl))
                    {
                        n++;
                    }
                }

                var drift = Convert.ToInt32((ttl.TotalMilliseconds * options.ClockDriveFactor) + 2);
                var validityTime = ttl - (DateTime.Now - startTime) - new TimeSpan(0, 0, 0, 0, drift);

                if (n >= Quorum && validityTime.TotalMilliseconds > 0)
                {
                    return LockResult.Create(new Mutex(resource, value, validityTime));
                }

                foreach (var connection in connections)
                {
                    await UnlockInstance(connection, resource, value);
                }

                return LockResult.Empty;
            }
            catch (Exception)
            {
                return LockResult.Empty;
            }
        }

        private static async Task<bool> LockInstance(ConnectionMultiplexer connection, object resource, byte[] value, TimeSpan ttl)
        {
            try
            {
                var key = GetRedisKey(resource);
                return await connection.GetDatabase().StringSetAsync(key, value, ttl, When.NotExists);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task UnlockInstance(ConnectionMultiplexer connection, object resource, byte[] value)
        {
            RedisKey[] key = { GetRedisKey(resource) };
            RedisValue[] values = { value };

            await connection.GetDatabase().ScriptEvaluateAsync(UnlockScript, key, values);
        }

        private static string GetRedisKey(object resource)
        {
            if (resource.GetType() == typeof(string))
            {
                return resource.ToString();
            }

            return JsonConvert.SerializeObject(resource);
        }

        private static async Task<LockResult> Retry(Func<Task<LockResult>> action, int retryCount, TimeSpan retryDelay)
        {
            var currentRetry = 0;
            var result = LockResult.Empty;

            while (currentRetry++ < retryCount)
            {
                result = await action();
                if (result.Locked) break;

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
