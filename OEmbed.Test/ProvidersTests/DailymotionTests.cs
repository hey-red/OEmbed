namespace OEmbed.Test.ProvidersTests;

public class DailymotionTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public DailymotionTests(ITestOutputHelper output)
    {
        _output = output;
        _oEmbedProvider = new DailymotionProvider();
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData("https://www.dailymotion.com/video/x87cx3z")]
    [InlineData("https://dai.ly/x87cx3z")]
    [InlineData("https://www.dailymotion.com/embed/video/x87cx3z?autoplay=1")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task RequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Video>("https://dai.ly/x87cx3z");

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