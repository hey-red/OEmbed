namespace OEmbed.Test.ProvidersTests
{
    public class GfycatTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public GfycatTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new GfycatProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://gfycat.com/remoteignorantfallowdeer")]
        [InlineData("https://gfycat.com/ifr/RemoteIgnorantFallowdeer")]
        [InlineData("https://thumbs.gfycat.com/RemoteIgnorantFallowdeer-mobile.mp4")]
        [InlineData("https://zippy.gfycat.com/RemoteIgnorantFallowdeer.mp4")]
        [InlineData("https://giant.gfycat.com/RemoteIgnorantFallowdeer.mp4")]
        public void GfycatMatchTest(string url)
        {
            Assert.True(_oEmbedProvider.CanProcess(new Uri(url)));
        }

        [Fact]
        public async void GfycatRequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Video>("https://gfycat.com/remoteignorantfallowdeer");

            Assert.NotNull(result);
            Assert.Equal("video", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.NotNull(result.Title);
            Assert.Null(result.AuthorName);
            Assert.Null(result.AuthorUrl);
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

            _output.WriteLine(result?.ProviderName);
            _output.WriteLine(result?.Title);
        }
    }
}