namespace OEmbed.Test.ProvidersTests;

public class FlickrTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public FlickrTests(ITestOutputHelper output)
    {
        _output = output;
        _oEmbedProvider = new FlickrProvider();
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData("https://www.flickr.com/photos/191258271@N04/51414360766/in/album-72157720098386911/")]
    [InlineData("https://flic.kr/p/2mkj3cY")]
    [InlineData("https://www.flickr.com/photos/191258271@N04/albums/72157720098386911")]
    [InlineData("https://www.flickr.com/photos/191258271@N04/sets/72157720098386911/")]
    [InlineData("https://www.flickr.com/photos/191258271@N04/")]
    [InlineData("https://www.flickr.com/photos/191258271@N04/favorites")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task RequestPhotoTest()
    {
        var result =
            await _oEmbedConsumer.RequestAsync<Photo>(
                "https://www.flickr.com/photos/lucian-f/49267518876/in/album-72157712341359412/");

        Assert.NotNull(result);
        Assert.Equal("photo", result!.Type);
        Assert.NotNull(result.Version);
        Assert.NotNull(result.Title);
        Assert.NotNull(result.AuthorName);
        Assert.NotNull(result.AuthorUrl);
        Assert.NotNull(result.ProviderName);
        Assert.NotNull(result.ProviderUrl);
        Assert.NotNull(result.CacheAge);
        Assert.NotNull(result.ThumbnailUrl);
        Assert.NotNull(result.ThumbnailWidth);
        Assert.NotNull(result.ThumbnailHeight);
        // Photo type values
        Assert.NotNull(result.Url);
        Assert.NotEqual(0, result.Width);
        Assert.NotEqual(0, result.Height);

        _output.WriteLine(result?.Title);
        _output.WriteLine(result?.Url);
    }

    [Fact]
    public async Task RequestAlbumTest()
    {
        var result =
            await _oEmbedConsumer.RequestAsync<Rich>("https://www.flickr.com/photos/lucian-f/albums/72157712341359412");

        Assert.NotNull(result);
        Assert.Equal("rich", result!.Type);
        Assert.NotNull(result.Version);
        Assert.NotNull(result.Title);
        Assert.NotNull(result.AuthorName);
        Assert.NotNull(result.AuthorUrl);
        Assert.NotNull(result.ProviderName);
        Assert.NotNull(result.ProviderUrl);
        Assert.NotNull(result.CacheAge);
        Assert.NotNull(result.ThumbnailUrl);
        Assert.NotNull(result.ThumbnailWidth);
        Assert.NotNull(result.ThumbnailHeight);
        // Rich type values
        Assert.NotNull(result.Html);
        Assert.NotEqual(0, result.Width);
        Assert.NotEqual(0, result.Height);

        _output.WriteLine(result?.Title);
        _output.WriteLine(result?.ThumbnailUrl);
    }
}