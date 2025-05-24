using System.Reflection;

namespace OEmbed.Test;

public static class TestHelpers
{
    private const string TEST_DIR = "Fixtures";
    private static readonly HttpClient HttpClient = new();

    public static OEmbedConsumer BuildConsumer(IList<IOEmbedProvider> providers, bool withCache = false)
    {
        var providerRegistry = new ProviderRegistry(providers);

        return new OEmbedConsumer(
            HttpClient,
            providerRegistry,
            options: new OEmbedOptions { EnableCache = withCache });
    }

    public static void UrlShouldMatchTest(IOEmbedProvider provider, string url)
    {
        UrlMatchTest(provider, url, true);
    }

    public static void UrlShouldNotMatchTest(IOEmbedProvider provider, string url)
    {
        UrlMatchTest(provider, url, false);
    }

    private static void UrlMatchTest(IOEmbedProvider provider, string url, bool shouldMatch)
    {
        var uri = new Uri(url, UriKind.Absolute);

        if (shouldMatch)
        {
            Assert.True(provider.CanProcess(uri));
            Assert.NotNull(provider.MatchScheme(uri));
        }
        else
        {
            bool canProcess = provider.CanProcess(uri);
            if (canProcess)
            {
                Assert.Null(provider.MatchScheme(uri));
            }
        }
    }

    public static string GetFixturesPath()
    {
        string assemblyLocation = typeof(TestHelpers).GetTypeInfo()
            .Assembly.Location;
        var assemblyFile = new FileInfo(assemblyLocation);
        DirectoryInfo? directoryInfo = assemblyFile.Directory;

        while (!directoryInfo!
                   .EnumerateDirectories(TEST_DIR)
                   .Any())
        {
            directoryInfo = directoryInfo.Parent;
        }

        return Path.Combine(directoryInfo.FullName, TEST_DIR);
    }
}