using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Elders.RedLock
{
    internal sealed class RedLockOptionsProvider : IConfigureOptions<RedLockOptions>
    {
        private readonly IConfiguration configuration;

        public RedLockOptionsProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Configure(RedLockOptions options)
        {
            configuration.GetSection("RedLock").Bind(options);

            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(options);
            var valid = Validator.TryValidateObject(options, context, validationResults, true);
            if (valid)
                return;

            var msg = string.Join("\n", validationResults.Select(r => r.ErrorMessage));
            throw new Exception($"Invalid configuration!':\n{msg}");
        }
    }
}
