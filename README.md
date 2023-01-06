# RedLock

## A C# implementation of a distributed lock algorithm with Redis based on [this documentation](https://redis.io/docs/manual/patterns/distributed-locks/)

### Setup

1. Register RedLock to your container
```csharp
services.AddRedLock();
```

2. Configure
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
| Setting | Comment | Default |
| --- | --- | :---: |
| ConnectionString | Your Redis connection string | (required) |
| LockRetryCount | Total amount of retries to aquire lock | 1 |
| LockRetryDelay | Time to wait between retries | 10 ms |
| ClockDriveFactor | Factor to determine the clock drift. `clock_drift = (lock_ttl_milliseconds * ClockDriveFactor) + 2`. Adding 2 milliseconds to the drift to account for Redis expires precision (1 ms) plus the configured allowable drift factor. Read [this](https://redis.io/docs/manual/patterns/distributed-locks/#safety-arguments) for details. | 0.01 |

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
