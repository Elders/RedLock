# [9.0.0-preview.1](https://github.com/Elders/RedLock/compare/v8.3.0...v9.0.0-preview.1) (2023-10-23)


### Bug Fixes

* Configures the CI to use net8 preview ([0be952b](https://github.com/Elders/RedLock/commit/0be952b80db5e6a64515b1c760eff8d8bc189f17))

# [8.3.0](https://github.com/Elders/RedLock/compare/v8.2.1...v8.3.0) (2023-10-23)


### Features

* Adds ExtendLockAsync ([23db9ab](https://github.com/Elders/RedLock/commit/23db9ab50d60b42fcf789497ff217fda7be701d6))
* extending lock duration ([851ffc6](https://github.com/Elders/RedLock/commit/851ffc60cb2c0a2b3ff2968aa0c702bc79e9c7a6))

# [8.3.0-preview.1](https://github.com/Elders/RedLock/compare/v8.2.1...v8.3.0-preview.1) (2023-02-15)


### Features

* extending lock duration ([851ffc6](https://github.com/Elders/RedLock/commit/851ffc60cb2c0a2b3ff2968aa0c702bc79e9c7a6))

## [8.2.1](https://github.com/Elders/RedLock/compare/v8.2.0...v8.2.1) (2023-01-11)


### Bug Fixes

* Improves logging ([23b917c](https://github.com/Elders/RedLock/commit/23b917c7a8897eebe60b4af0bb6ee1a7f10618d8))

# [8.2.0](https://github.com/Elders/RedLock/compare/v8.1.0...v8.2.0) (2023-01-09)


### Features

* Allows overriding the default config options provider ([525edc9](https://github.com/Elders/RedLock/commit/525edc9a9f4e55d5b1ecfe0c860d704f7152792d))

# [8.1.0](https://github.com/Elders/RedLock/compare/v8.0.0...v8.1.0) (2023-01-06)


### Bug Fixes

* bind Redlock options from the configurations ([99aefa1](https://github.com/Elders/RedLock/commit/99aefa15dc839e71dfc02fe182ad476a8599b914))
* validate options ([b7693ac](https://github.com/Elders/RedLock/commit/b7693ac48cb85c306ed37cdc095be52e1c9e01e0))


### Features

* making the logger optional ([ae69a92](https://github.com/Elders/RedLock/commit/ae69a924d328120f82055668d09fc9c3966cb75f))

# [8.1.0-preview.2](https://github.com/Elders/RedLock/compare/v8.1.0-preview.1...v8.1.0-preview.2) (2023-01-06)


### Bug Fixes

* validate options ([b7693ac](https://github.com/Elders/RedLock/commit/b7693ac48cb85c306ed37cdc095be52e1c9e01e0))

# [8.1.0-preview.1](https://github.com/Elders/RedLock/compare/v8.0.0...v8.1.0-preview.1) (2023-01-06)


### Bug Fixes

* bind Redlock options from the configurations ([99aefa1](https://github.com/Elders/RedLock/commit/99aefa15dc839e71dfc02fe182ad476a8599b914))


### Features

* making the logger optional ([ae69a92](https://github.com/Elders/RedLock/commit/ae69a924d328120f82055668d09fc9c3966cb75f))

# [8.0.0](https://github.com/Elders/RedLock/compare/v7.0.1...v8.0.0) (2023-01-06)


### Bug Fixes

* Fixes the retry to work properly ([7c00d3d](https://github.com/Elders/RedLock/commit/7c00d3ddff5928ed4f6e7797905914acca827b96))

# [8.0.0-preview.2](https://github.com/Elders/RedLock/compare/v8.0.0-preview.1...v8.0.0-preview.2) (2023-01-06)


### Bug Fixes

* Fixes the retry to work properly ([7c00d3d](https://github.com/Elders/RedLock/commit/7c00d3ddff5928ed4f6e7797905914acca827b96))

# [8.0.0-preview.1](https://github.com/Elders/RedLock/compare/v7.0.1...v8.0.0-preview.1) (2023-01-05)

## [7.0.1](https://github.com/Elders/RedLock/compare/v7.0.0...v7.0.1) (2022-08-16)


### Bug Fixes

* pipeline update ([d01b3e1](https://github.com/Elders/RedLock/commit/d01b3e10b86b14a8fb833acd7df7bd95982c4904))

# [7.0.0](https://github.com/Elders/RedLock/compare/v6.1.0...v7.0.0) (2022-05-16)

# [6.1.0](https://github.com/Elders/RedLock/compare/v6.0.0...v6.1.0) (2022-03-28)


### Features

* Changes Logger ([27ecc6f](https://github.com/Elders/RedLock/commit/27ecc6fd070dc38971c70cde82da74a0e3b0e4eb))

# [6.0.0](https://github.com/Elders/RedLock/compare/v5.0.0...v6.0.0) (2022-03-28)

### 5.0.0 - 09.04.2020
* Using Options pattern

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
