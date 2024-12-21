namespace OEmbed.Test.ProvidersTests
{
    public class AnnieMusicTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public AnnieMusicTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new AnnieMusicProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://anniemusic.app/t/1234")]
        [InlineData("https://anniemusic.app/t/vqf1Geraao")]
        [InlineData("https://anniemusic.app/t/1416?utm_campaign=SasaStation%27s%20Music%20Newsletter&utm_medium=email&utm_source=Revue%20newsletter")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async Task RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://anniemusic.app/t/1234");

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
            Assert.Null(result.ThumbnailWidth);
            Assert.Null(result.ThumbnailHeight);
            // Rich type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.Title);
        }
    }
}