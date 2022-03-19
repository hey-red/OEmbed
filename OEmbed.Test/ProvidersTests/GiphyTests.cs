namespace OEmbed.Test.ProvidersTests
{
    public class GiphyTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public GiphyTests(ITestOutputHelper output)
        {
            _output = output;
            _oEmbedProvider = new GiphyProvider();
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://giphy.com/gifs/confused-iu-looking-up-l2YWwjl8T5tdGiaf6")]
        [InlineData("https://www.giphy.com/gifs/confused-iu-looking-up-l2YWwjl8T5tdGiaf6")]
        [InlineData("https://media.giphy.com/media/l2YWwjl8T5tdGiaf6/giphy.gif")]
        [InlineData("https://giphy.com/media/l2YWwjl8T5tdGiaf6/giphy.gif")]
        [InlineData("https://giphy.com/stickers/iu-dlwlrma-uaena-PslxPYDZKuD1vNYWJy")]
        [InlineData("https://giphy.com/clips/kpop-k-pop-red-velvet-UWq9DlocbqoIal7N7I")]
        [InlineData("http://giphy.com/embed/l2YWwjl8T5tdGiaf6")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async void RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Photo>("https://giphy.com/gifs/confused-iu-looking-up-l2YWwjl8T5tdGiaf6");

            Assert.NotNull(result);
            Assert.Equal("photo", result!.Type);
            Assert.Null(result.Version); // Giphy omits version
            Assert.NotNull(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.NotNull(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);
            Assert.Null(result.CacheAge);
            Assert.Null(result.ThumbnailUrl);
            Assert.Null(result.ThumbnailWidth);
            Assert.Null(result.ThumbnailHeight);
            // Photo type values
            Assert.NotNull(result.Url);
            Assert.NotEqual(0, result.Width);
            Assert.NotEqual(0, result.Height);

            _output.WriteLine(result?.AuthorName);
            _output.WriteLine(result?.Title);
        }

        [Fact]
        public async void DynamicResponseTest()
        {
            var result = await _oEmbedConsumer.RequestAsync("https://giphy.com/clips/kpop-k-pop-red-velvet-UWq9DlocbqoIal7N7I");

            Assert.IsType<Video>(result);
        }
    }
}