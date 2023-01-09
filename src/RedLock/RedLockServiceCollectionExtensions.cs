using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Elders.RedLock
{
    public static class RedLockServiceCollectionExtensions
    {
        public static IServiceCollection AddRedLock(this IServiceCollection services)
        {
            services.AddSingleton<IRedisLockManager, RedisLockManager>();

            services.AddOptions<RedLockOptions>();
            services.AddSingleton<IConfigureOptions<RedLockOptions>, RedLockOptionsProvider>();

            return services;
        }

        public static IServiceCollection AddRedLock<TOptionsProvider>(this IServiceCollection services)
            where TOptionsProvider : class, IConfigureOptions<RedLockOptions>
        {
            services.AddSingleton<IRedisLockManager, RedisLockManager>();

            services.AddOptions<RedLockOptions>();
            services.AddSingleton<IConfigureOptions<RedLockOptions>, TOptionsProvider>();

            return services;
        }
    }
}
