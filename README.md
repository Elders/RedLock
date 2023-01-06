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
            "ConnectionString": "{YOUR_REDIS_CONNECTION_STRING}",
            "LockRetryCount": 2,
            "LockRetryDelay": "00:00:00.500",
            "ClockDriveFactor": 0.02
        }
    }
    ```
| Setting | Comment |
| --- | --- |
| ConnectionString | Your Redis connection string (required) |
| LockRetryCount | Total amount of retries to aquire lock (default: 1) |
| LockRetryDelay | Time to wait between retries (default: 10 ms.) |
| ClockDriveFactor | Factor to determine the clock drift. Read [this](https://redis.io/docs/manual/patterns/distributed-locks/#safety-arguments) for details. `clock_drift = (lock_ttl_milliseconds * ClockDriveFactor) + 2`. (default: 0.01) |

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
