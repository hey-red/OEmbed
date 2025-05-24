using System.Collections.Generic;
using System.Linq;

using HeyRed.OEmbed;
using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Providers;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace OEmbed.Extensions.Microsoft.DependencyInjection.Tests;

public class ProviderRegistrationTests
{
    [Fact]
    public void BasicRegistrationTest()
    {
        var services = new ServiceCollection();

        services
            .AddOEmbed()
            .ClearProviders()
            .AddProvider<TwitterProvider>()
            .AddProvider<TiktokProvider>();

        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        var oEmbedProviders = serviceProvider
            .GetServices<IOEmbedProvider>()
            .ToList();

        Assert.Equal(2, oEmbedProviders.Count);
        Assert.IsType<TwitterProvider>(oEmbedProviders[0]);
        Assert.IsType<TiktokProvider>(oEmbedProviders[1]);
    }

    [Fact]
    public void ProviderOptionsConfigurationTest()
    {
        var services = new ServiceCollection();

        services
            .AddOEmbed()
            .ClearProviders()
            .AddProvider<TwitterProvider>(options =>
            {
                options.Parameters = new Dictionary<string, string?>
                {
                    ["theme"] = "dark"
                };
            });

        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        var provider = serviceProvider.GetRequiredService<IOEmbedProvider>();

        var parameter = provider.Parameters.First();
        Assert.Equal("theme", parameter.Key);
        Assert.Equal("dark", parameter.Value);
    }
}