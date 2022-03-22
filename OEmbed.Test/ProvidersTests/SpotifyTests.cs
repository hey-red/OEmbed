namespace OEmbed.Test.ProvidersTests
{
    public class SpotifyTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public SpotifyTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new SpotifyProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://open.spotify.com/artist/3HqSLMAZ3g3d5poNaI7GOU")]
        [InlineData("https://open.spotify.com/album/2xEH7SRzJq7LgA0fCtTlxH")]
        [InlineData("https://open.spotify.com/track/4Dr2hJ3EnVh2Aaot6fRwDO?si=66ea03b84a8940b5")]
        [InlineData("https://open.spotify.com/playlist/37i9dQZF1DX0y9CwEpdGpz")]
        [InlineData("https://open.spotify.com/show/6RLX4Ns3kRUQiJi7RZl4NA")]
        [InlineData("https://open.spotify.com/episode/5xpsJbqTd09Lk5fZocYeOw")]
        [InlineData("https://open.spotify.com/user/spotify/playlist/37i9dQZF1DX0y9CwEpdGpz")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        /// <summary>
        /// NOTE: spotify requires user-agent header
        /// </summary>
        [Fact]
        public async void RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://open.spotify.com/artist/3HqSLMAZ3g3d5poNaI7GOU");

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.NotNull(result.Title);
            Assert.Null(result.AuthorName);
            Assert.Null(result.AuthorUrl);
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
            _output.WriteLine(result?.ProviderName);
        }
    }
}