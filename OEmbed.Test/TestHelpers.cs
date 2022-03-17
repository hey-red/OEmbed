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