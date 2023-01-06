# Redlock

## A C# implementation of a distributed lock with Redis based on [this documentation](https://redis.io/docs/manual/patterns/distributed-locks/)

### Setup

1. `Startup.cs`
    ```csharp
    services.AddRedLock();
    ```

2. `appsettings.json`
    ```json
    {
        "RedLock": {
            "ConnectionString": "{YOUR_REDIS_CONNECTION_STRING}", // required
            "LockRetryCount": 2, // defaults to 1
            "LockRetryDelay": "00:00:00.500", // defaults to 10 ms
            "ClockDriveFactor": 0.02 // defaults to 0.01 (Read https://redis.io/docs/manual/patterns/distributed-locks/#safety-arguments for details)
        }
    }
    ```

### Usage

```csharp
var redlock = serviceProvider.GetRequiredService<IRedisLockManager>();

var resource = "resource_key";
if (await redlock.LockAsync(resource, TimeSpan.FromSeconds(2)))
{
    try
    {
        // do stuff
        if (await redlock.IsLockedAsync(resource))
        {
            // do more stuff
        }
        else
        {
            // do something else
        }
    }
    finally
    {
        await redlock.UnlockAsync(resource);
    }
}
else
{
    // failed to lock resource
    // fallback
}
```
