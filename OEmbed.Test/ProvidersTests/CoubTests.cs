namespace OEmbed.Test.ProvidersTests;

public class CoubTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public CoubTests(ITestOutputHelper output)
    {
        _output = output;
        _oEmbedProvider = new CoubProvider();
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData("https://coub.com/view/utw6y")]
    [InlineData("https://coub.com/embed/utw6y")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task RequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Video>("https://coub.com/view/utw6y");

        Assert.NotNull(result);
        Assert.Equal("video", result!.Type);
        Assert.Equal("1.0", result.Version);
        Assert.NotNull(result.Title);
        Assert.NotNull(result.AuthorName);
        Assert.Null(result.AuthorUrl);
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