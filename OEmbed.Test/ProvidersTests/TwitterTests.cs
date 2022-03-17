﻿namespace OEmbed.Test.ProvidersTests
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
        public void TwitterMatchTest(string url)
        {
            Assert.True(_oEmbedProvider.CanProcess(new Uri(url)));
        }

        [Fact]
        public async void TwitterRequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://twitter.com/panpianoatelier/status/1500450869590241286", maxWidth: 400);

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
        public async void TwitterDynamicResponse()
        {
            var result = await _oEmbedConsumer.RequestAsync("https://twitter.com/panpianoatelier/status/1500450869590241286");

            Assert.IsType<Rich>(result);
        }
    }
}