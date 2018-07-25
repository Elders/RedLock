http://redis.io/topics/distlock

# Example
```
class Program
{
    static void Main(string[] args)
    {
        var lockManager = new RedisLockManager("docker-local.com:6379,abortConnect=False");

        var resource = "resource_key";

        if (lockManager.Lock(resource, TimeSpan.FromSeconds(60)))
        {
            try
            {
                // do stuff
            }
            finally
            {
                lockManager.Unlock(resource);
            }
        }
    }
}
```
