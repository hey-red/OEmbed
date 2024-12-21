namespace OEmbed.Test.ProvidersTests
{
    public class TwitterTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public TwitterTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new TwitterProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("http://twitter.com/panpianoatelier/status/1500450869590241286")]
        [InlineData("http://www.twitter.com/panpianoatelier/status/1500450869590241286")]
        [InlineData("https://twitter.com/panpianoatelier/status/1500450869590241286")]
        [InlineData("https://mobile.twitter.com/panpianoatelier/status/1500450869590241286")]
        [InlineData("https://twitter.com/panpianoatelier/status/1500450869590241286?s=20&t=piEth1McNILTUdD9Tf-48w")]
        [InlineData("https://twitter.com/N_I_X_E_U")]
        [InlineData("https://twitter.com/N_I_X_E_U/likes")]
        [InlineData("https://twitter.com/i/lists/811600711968575488")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async Task LikesRequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://twitter.com/N_I_X_E_U/likes", maxWidth: 400);

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.NotNull(result.Title);
            Assert.Null(result.AuthorName);
            Assert.Null(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);

            Assert.NotNull(result.CacheAge);
            Assert.NotNull(result.Html);

            Assert.Null(result.ThumbnailUrl);
            Assert.Null(result.ThumbnailWidth);
            Assert.Null(result.ThumbnailHeight);

            Assert.Equal(400, result.Width);
            Assert.Null(result.Height);

            _output.WriteLine(result?.ProviderName);
            _output.WriteLine(result?.ProviderUrl);
        }

        [Fact]
        public async Task RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://twitter.com/panpianoatelier/status/1500450869590241286?s=20&t=piEth1McNILTUdD9Tf-48w", maxWidth: 400);

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

            Assert.Equal(400, result.Width);
            Assert.Null(result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.AuthorUrl);
        }

        [Fact]
        public async Task RequestWithAdditionalParametersTest()
        {
            var provider = new TwitterProvider(new()
            {
                Parameters = new Dictionary<string, string?>()
                {
                    ["theme"] = "dark"
                }
            });

            var consumer = TestHelpers.BuildConsumer(new[] { provider });
            var result = await consumer.RequestAsync<Rich>("https://twitter.com/panpianoatelier/status/1500450869590241286");

            Assert.NotNull(result);
            Assert.Contains("data-theme=\"dark\"", result!.Html);
        }
    }
}