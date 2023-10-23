using Elders.RedLock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();
services.AddRedLock();
services.AddSingleton<IConfiguration>(configuration);

var provider = services.BuildServiceProvider();
var redlock = provider.GetRequiredService<IRedisLockManager>();

var resource = "resource_key";
if (await redlock.LockAsync(resource, TimeSpan.FromSeconds(20)))
{
    try
    {
        // do stuff
        Console.WriteLine("locked");
        if (await redlock.IsLockedAsync(resource))
        {
            // do more stuff
            Console.WriteLine("still locked");
            await Task.Delay(TimeSpan.FromSeconds(5));
            if (await redlock.ExtendLockAsync(resource, TimeSpan.FromSeconds(20)))
            {
                // do even more stuff
                Console.WriteLine("lock extended");
                await Task.Delay(TimeSpan.FromSeconds(5));
                if (await redlock.IsLockedAsync(resource))
                {
                    Console.WriteLine("still locked");
                }
            }
            else
            {
                // failed to extend resource lock
                // fallback
                Console.WriteLine("failed to extend");
            }
        }
        else
        {
            // do something else
            Console.WriteLine("not locked anymore");
        }
    }
    finally
    {
        await redlock.UnlockAsync(resource);
        Console.WriteLine("unlocked");
    }
}
else
{
    // failed to lock resource
    // fallback
    Console.WriteLine("lock failed");
}
