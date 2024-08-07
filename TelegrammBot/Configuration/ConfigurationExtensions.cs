﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegrammBot.Configuration
{
    public static class ConfigurationExtensions
    {
        public static T GetConfiguration<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            var o = serviceProvider.GetService<IOptions<T>>() ??
                throw new ArgumentNullException(nameof(T));

            return o.Value;
        }
    }
}
