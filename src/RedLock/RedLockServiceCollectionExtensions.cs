using Microsoft.Extensions.DependencyInjection;

namespace Elders.RedLock
{
    public static class RedLockServiceCollectionExtensions
    {
        public static IServiceCollection AddRedLock(this IServiceCollection services)
        {
            services.AddSingleton<IRedisLockManager, RedisLockManager>();

            services.AddOptions<RedLockOptions>();

            return services;
        }
    }
}
