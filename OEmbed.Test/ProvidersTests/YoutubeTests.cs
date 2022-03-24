namespace OEmbed.Test.ProvidersTests
{
    public class YoutubeTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public YoutubeTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new YoutubeProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://youtu.be/LKWFkELeYwc")]
        [InlineData("http://youtu.be/LKWFkELeYwc")]
        [InlineData("https://www.youtu.be/LKWFkELeYwc")]
        [InlineData("https://www.youtube.com/watch?v=LKWFkELeYwc")]
        [InlineData("https://www.youtube.com/embed/LKWFkELeYwc?autoplay=1")]
        [InlineData("https://m.youtube.com/watch?v=LKWFkELeYwc&feature=youtu.be")]
        [InlineData("https://youtu.be/LKWFkELeYwc?t=5")]
        [InlineData("https://www.youtube.com/watch?v=LKWFkELeYwc&t=5")]
        [InlineData("https://www.youtube.com/watch?v=LKWFkELeYwc&t=5s")]
        [InlineData("https://www.youtube.com/watch?v=GxmJBAIoWUo&list=PL8mPWv3h4qJcOYZn8iFMLga3DjAY5nQLQ")]
        [InlineData("https://www.youtube.com/playlist?list=PL8mPWv3h4qJcOYZn8iFMLga3DjAY5nQLQ")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async void RequestTest()
        {
            Video? result = await _oEmbedConsumer.RequestAsync<Video>("https://www.youtube.com/watch?v=LKWFkELeYwc");

            Assert.NotNull(result);
            Assert.Equal("video", result!.Type);
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
            // Video type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.Title);
        }
    }
}