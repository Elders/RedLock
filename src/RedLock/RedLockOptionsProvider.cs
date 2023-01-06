using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Elders.RedLock
{
    public sealed class RedLockOptionsProvider : IConfigureOptions<RedLockOptions>
    {
        private readonly IConfiguration configuration;

        public RedLockOptionsProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Configure(RedLockOptions options)
        {
            configuration.GetSection("RedLock").Bind(options);
        }
    }
}
