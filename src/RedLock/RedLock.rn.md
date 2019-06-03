### 4.0.1 - 17.06.2019
* Update StackExchange.Redis to 2.0.601
* Update LibLog to 5.0.6
* Update copy-right attribute

#### 4.0.0 - 12.10.2018
* Updates StackExchange.Redis package major version
* Updates LibLog package major version
* Fixes the project file such as nuget info, assembly name, root namespace and adds SourceLink

#### 3.0.1 - 13.08.2018
* Adds ConfigureAwait to async calls

#### 3.0.0 - 25.07.2018
* Removes Newtonsoft.Json
* Removes LockResult
* Removes Mutex

#### 2.0.1 - 20.02.2018
* Downgrades Newtonsoft.Json to 10.0.3

#### 2.0.0 - 19.02.2018
* Adds netstandard2.0 support

#### 1.0.1 - 27.01.2016
* Changed RedisLockManager constructor to work with Redis Connection String


#### 1.0.0 - 27.01.2016
* Changed RedisLockManager constructor to work with StackExchange.Redis.ConnectionOptions instead with IPEndpoints 

#### 0.1.0 - 07.01.2016
* Use a 64-based string as a Redis key if an object is supplied as a resource key.
* Update RedisLockManager to use Redis cluster.
* RedisLockManager.UnlockInstance does not throw if the end point is unreachable.
* Added dependency to LibLog.
* Exceptions from LockInstance(), UnlockInstance() and IsLocked() are being logged.
