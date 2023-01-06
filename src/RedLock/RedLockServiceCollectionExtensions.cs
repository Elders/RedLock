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
            services.AddTransient<IConfigureOptions<RedLockOptions>, RedLockOptionsProvider>();

            return services;
        }
    }
}
