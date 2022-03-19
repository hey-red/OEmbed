using System.Reflection;

namespace OEmbed.Test
{
    public static class TestHelpers
    {
        private static readonly HttpClient HttpClient = new();

        public static OEmbedConsumer BuildConsumer(IList<IOEmbedProvider> providers)
        {
            var providerRegistry = new ProviderRegistry(providers);

            return new OEmbedConsumer(
                httpClient: HttpClient,
                providerRegistry: providerRegistry,
                options: new OEmbedOptions { EnableCache = false });
        }

        public static void UrlShouldMatchTest(IOEmbedProvider provider, string url) => UrlMatchTest(provider, url, true);

        public static void UrlShouldNotMatchTest(IOEmbedProvider provider, string url) => UrlMatchTest(provider, url, false);

        private static void UrlMatchTest(IOEmbedProvider provider, string url, bool shouldMatch)
        {
            var uri = new Uri(url);

            if (shouldMatch)
            {
                Assert.True(provider.CanProcess(uri));
                Assert.Contains(provider.Schemes, scheme => scheme.Key.IsMatch(uri));
            }
            else
            {
                Assert.DoesNotContain(provider.Schemes, scheme => scheme.Key.IsMatch(uri));
            }
        }

        private const string TEST_DIR = "Fixtures";

        public static string GetFixturesPath()
        {
            var assemblyLocation = typeof(TestHelpers).GetTypeInfo().Assembly.Location;
            var assemblyFile = new FileInfo(assemblyLocation);
            var directoryInfo = assemblyFile.Directory;

            while (!directoryInfo!.EnumerateDirectories(TEST_DIR).Any())
            {
                directoryInfo = directoryInfo.Parent;
            }

            return Path.Combine(directoryInfo.FullName, TEST_DIR);
        }
    }
}