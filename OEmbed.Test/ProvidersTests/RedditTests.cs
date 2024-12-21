namespace OEmbed.Test.ProvidersTests
{
    public class RedditTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public RedditTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new RedditProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://www.reddit.com/r/SoulsSliders/comments/t7bsvp/someone_requested_kristen_ritter_figured_id_post/")]
        [InlineData("https://www.reddit.com/r/SoulsSliders/comments/t7bsvp/someone_requested_kristen_ritter_figured_id_post/?utm_source=share&utm_medium=web2x&context=3")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async Task RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://www.reddit.com/r/SoulsSliders/comments/t7bsvp/someone_requested_kristen_ritter_figured_id_post/");

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            //Assert.Equal("1.0", result.Version);
            Assert.NotNull(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.Null(result.AuthorUrl);
            Assert.Equal("reddit", result.ProviderName);
            Assert.Equal("https://www.reddit.com", result.ProviderUrl);
            Assert.Null(result.CacheAge);
            Assert.Null(result.ThumbnailUrl);
            Assert.Null(result.ThumbnailWidth);
            Assert.Null(result.ThumbnailHeight);
            // Rich type values
            Assert.NotNull(result.Html);
            Assert.Null(result.Width);
            Assert.NotNull(result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.Title);
        }
    }
}