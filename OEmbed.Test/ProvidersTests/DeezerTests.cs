namespace OEmbed.Test.ProvidersTests
{
    public class DeezerTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public DeezerTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new DeezerProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://deezer.page.link/ogt5zwbUePw8GCDx9")]
        [InlineData("https://www.deezer.com/us/album/217565572")]
        [InlineData("https://www.deezer.com/us/track/1286394242")]
        [InlineData("https://www.deezer.com/us/playlist/8664185942")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async Task RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://deezer.page.link/ogt5zwbUePw8GCDx9");

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.NotNull(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.Null(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);
            Assert.NotNull(result.CacheAge);
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