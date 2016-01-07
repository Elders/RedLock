#### 0.1.0 - 07.01.2016
* Use a 64-based string as a Redis key if an object is supplied as a resource key.
* Update RedisLockManager to use Redis cluster.
* RedisLockManager.UnlockInstance does not throw if the end point is unreachable.
* Added dependency to LibLog.
* Exceptions from LockInstance(), UnlockInstance() and IsLocked() are being logged.