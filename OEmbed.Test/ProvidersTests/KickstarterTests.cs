namespace OEmbed.Test.ProvidersTests
{
    public class KickstarterTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public KickstarterTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new KickstarterProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://www.kickstarter.com/projects/brentonlengel/snow-white-zombie-apocalypse-volume-1?ref=section-comics-illustration-featured-project")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async void RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://www.kickstarter.com/projects/brentonlengel/snow-white-zombie-apocalypse-volume-1?ref=section-comics-illustration-featured-project");

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.NotNull(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.NotNull(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);
            Assert.Null(result.CacheAge);
            Assert.NotNull(result.ThumbnailUrl);
            Assert.NotNull(result.ThumbnailWidth);
            Assert.NotNull(result.ThumbnailHeight);
            // Rich type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.Title);
            _output.WriteLine(result?.AuthorName);
        }
    }
}