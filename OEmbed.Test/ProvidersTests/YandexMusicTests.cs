namespace OEmbed.Test.ProvidersTests
{
    public class YandexMusicTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public YandexMusicTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new YandexMusicProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://music.yandex.ru/album/21260060")]
        [InlineData("https://music.yandex.ru/album/21260060/track/101006830")]
        [InlineData("https://music.yandex.ru/track/101006830")]
        [InlineData("https://music.yandex.ru/users/music-blog/playlists/2127")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async void RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://music.yandex.ru/track/101006830");

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
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
            // Rich type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.Title);
        }
    }
}