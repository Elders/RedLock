using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RedLock;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var endpoint1 = new IPEndPoint(IPAddress.Parse("192.168.99.100"), 32768);
            var endpoint2 = new IPEndPoint(IPAddress.Parse("192.168.99.100"), 32769);
            var endpoint3 = new IPEndPoint(IPAddress.Parse("192.168.99.100"), 32770);
            var endpoint4 = new IPEndPoint(IPAddress.Parse("192.168.99.100"), 32785);
            var endpoint5 = new IPEndPoint(IPAddress.Parse("192.168.99.100"), 32772);
            var endpoints = new[] { endpoint1, endpoint2, endpoint3, endpoint4, endpoint5 };

            var lockManager = new RedisLockManager(endpoints);

            var result = lockManager.Lock("asdf", TimeSpan.FromSeconds(60));

            if (result.Locked)
            {
                lockManager.Unlock(result.Mutex);
            }
        }
    }
}
