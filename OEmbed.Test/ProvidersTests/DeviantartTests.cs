namespace OEmbed.Test.ProvidersTests
{
    public class DeviantartTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public DeviantartTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new DeviantartProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://www.deviantart.com/wlop/art/Sunshine2-905829772")]
        [InlineData("https://www.deviantart.com/art/Sunshine2-905829772")]
        [InlineData("https://sta.sh/015rpbp0snzv")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async void RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Photo>("https://www.deviantart.com/wlop/art/Sunshine2-905829772");

            Assert.NotNull(result);
            Assert.Equal("photo", result!.Type);
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
            // Photo type values
            Assert.NotNull(result.Url);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.Title);
        }
    }
}