using System;

using HeyRed.OEmbed.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeyRed.OEmbed
{
    public static class OEmbedBuilderExtensions
    {
        public static IOEmbedBuilder AddProvider<TProvider>(this IOEmbedBuilder builder)
            where TProvider : class, IOEmbedProvider
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOEmbedProvider, TProvider>());
            return builder;
        }

        public static IOEmbedBuilder ClearProviders(this IOEmbedBuilder builder)
        {
            builder.Services.RemoveAll(typeof(IOEmbedProvider));
            return builder;
        }

        public static IOEmbedBuilder SetCache<TCache>(this IOEmbedBuilder builder)
            where TCache : class, ICache
        {
            var descriptor = new ServiceDescriptor(typeof(ICache), typeof(TCache), ServiceLifetime.Singleton);
            builder.Services.Replace(descriptor);
            return builder;
        }

        public static IOEmbedBuilder SetCacheKey<T>(this IOEmbedBuilder builder)
            where T : class, ICacheKey
        {
            var descriptor = new ServiceDescriptor(typeof(ICacheKey), typeof(T), ServiceLifetime.Singleton);
            builder.Services.Replace(descriptor);
            return builder;
        }

        public static IOEmbedBuilder Configure<TOptions>(this IOEmbedBuilder builder, Action<TOptions> configureOptions)
             where TOptions : class
        {
            builder.Services.Configure(configureOptions);
            return builder;
        }
    }
}