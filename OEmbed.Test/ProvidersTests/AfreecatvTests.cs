namespace OEmbed.Test.ProvidersTests
{
    public class AfreecatvTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public AfreecatvTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new AfreecatvProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://vod.afreecatv.com/player/83325433")]
        [InlineData("https://play.afreecatv.com/whalsrud0217/239656993")]
        [InlineData("https://vod.afreecatv.com/PLAYER/STATION/84006619")]
        [InlineData("https://vod.sooplive.co.kr/player/159661333")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        private async Task RequestTest()
        {
            Video? result = await _oEmbedConsumer.RequestAsync<Video>("https://vod.afreecatv.com/player/83325433");

            Assert.NotNull(result);
            Assert.Equal("video", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.NotNull(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.NotNull(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);
            Assert.Null(result.CacheAge);
            Assert.Null(result.ThumbnailUrl);
            Assert.Null(result.ThumbnailWidth);
            Assert.Null(result.ThumbnailHeight);
            // Video type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.Title);
        }
    }
}