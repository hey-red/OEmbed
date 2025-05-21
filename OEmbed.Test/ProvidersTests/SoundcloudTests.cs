namespace OEmbed.Test.ProvidersTests
{
    public class SoundcloudTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public SoundcloudTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new SoundcloudProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://soundcloud.com/blackpinkofficial")]
        [InlineData("https://soundcloud.com/blackpinkofficial/whistle")]
        [InlineData("https://soundcloud.com/blackpinkofficial?utm_source=clipboard&utm_medium=text&utm_campaign=social_sharing")]
        [InlineData("https://on.soundcloud.com/182FhBVB9aLTZEuQ8")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Theory]
        [InlineData("https://soundcloud.com/discover")]
        [InlineData("https://soundcloud.com/upload")]
        public void UrlShouldNotMatchTest(string url)
        {
            TestHelpers.UrlShouldNotMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async Task RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://soundcloud.com/blackpinkofficial");

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
            Assert.Null(result.ThumbnailWidth);
            Assert.Null(result.ThumbnailHeight);
            // Rich type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.Title);
            _output.WriteLine(result?.AuthorUrl);
        }
    }
}