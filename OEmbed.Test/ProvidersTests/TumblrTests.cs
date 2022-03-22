namespace OEmbed.Test.ProvidersTests
{
    public class TumblrTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public TumblrTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new TumblrProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://hersheay.tumblr.com/post/679382416460283904/the-way-irene-looks-at-seulgi")]
        [InlineData("https://shibasommelier.tumblr.com/post/676286044432367616/2016-azienda-agricola-cos-cerasuolo-di-vittoria")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async void RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://hersheay.tumblr.com/post/679382416460283904/the-way-irene-looks-at-seulgi");

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.Null(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.NotNull(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);

            Assert.NotNull(result.CacheAge);
            Assert.NotNull(result.Html);

            Assert.Null(result.ThumbnailUrl);
            Assert.Null(result.ThumbnailWidth);
            Assert.Null(result.ThumbnailHeight);

            Assert.Equal(540, result.Width);
            Assert.Null(result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.AuthorUrl);
        }
    }
}