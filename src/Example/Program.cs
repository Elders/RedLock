using System;
using System.Net;
using RedLock;

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

            var lockManager = new RedisLockManager(endpoints);

            var result = lockManager.Lock("asdf", TimeSpan.FromSeconds(60));

            if (result.Locked)
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
