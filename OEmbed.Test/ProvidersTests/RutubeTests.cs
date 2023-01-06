namespace OEmbed.Test.ProvidersTests
{
    public class RutubeTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public RutubeTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new RutubeProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://rutube.ru/video/755ec2d4b19d2374d6c78890dbaad7fe/")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async void RequestVideoTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Video>("https://rutube.ru/video/755ec2d4b19d2374d6c78890dbaad7fe/");

            Assert.NotNull(result);
            Assert.Equal("video", result!.Type);
            Assert.Equal("1", result.Version); // Spec violation
            Assert.NotNull(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.NotNull(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);
            Assert.NotNull(result.CacheAge);
            Assert.NotNull(result.ThumbnailUrl);
            Assert.NotNull(result.ThumbnailWidth);
            Assert.NotNull(result.ThumbnailHeight);
            // Video type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.Title);
        }
    }
}
