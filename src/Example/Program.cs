using System;
using System.Net;
using RedLock;
using StackExchange.Redis;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpoint1 = new IPEndPoint(IPAddress.Parse("192.168.99.100"), 1001);
            var endpoint2 = new IPEndPoint(IPAddress.Parse("192.168.99.100"), 1002);
            var endpoint3 = new IPEndPoint(IPAddress.Parse("192.168.99.100"), 1003);

            var endpoints = new[] { endpoint1, endpoint2, endpoint3 };

            var config = ConfigurationOptions.Parse("docker-local.com:6379,abortConnect=False");

            var lockManager = new RedisLockManager(config);

            var result = lockManager.Lock("resource_key", TimeSpan.FromSeconds(60));

            if (result.LockAcquired)
            {
                try
                {
                    // do stuff
                }
                finally
                {
                    lockManager.Unlock(result.Mutex);
                }
            }
        }
    }
}
