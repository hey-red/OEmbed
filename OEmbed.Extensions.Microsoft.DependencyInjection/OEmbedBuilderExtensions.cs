using System;
using System.Reflection;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Providers.Common;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HeyRed.OEmbed;

public static class OEmbedBuilderExtensions
{
    public static IOEmbedBuilder AddProvider<TProvider>(this IOEmbedBuilder builder)
        where TProvider : class, IOEmbedProvider
    {
        return AddProvider<TProvider>(builder, _ => { });
    }

    public static IOEmbedBuilder AddProvider<TProvider>(
        this IOEmbedBuilder builder,
        Action<ProviderOptions> providerOptionsAction)
        where TProvider : class, IOEmbedProvider
    {
        Type providerType = typeof(TProvider);

        // Find constructor with options
        if (providerType.GetConstructor(BindingFlags.Public | BindingFlags.Instance,
                new[] { typeof(ProviderOptions) }) !=
            null)
        {
            ProviderOptions options = new();
            providerOptionsAction.Invoke(options);

            return AddProvider(builder, _ => (TProvider)Activator.CreateInstance(providerType, options)!);
        }

        return AddProvider(builder, _ => (TProvider)Activator.CreateInstance(providerType)!);
    }

    public static IOEmbedBuilder AddProvider<TProcessor>(
        this IOEmbedBuilder builder,
        Func<IServiceProvider, TProcessor> implementationFactory)
        where TProcessor : class, IOEmbedProvider
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOEmbedProvider>(implementationFactory));
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