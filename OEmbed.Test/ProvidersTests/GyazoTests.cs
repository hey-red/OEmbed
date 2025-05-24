namespace OEmbed.Test.ProvidersTests;

public class GyazoTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public GyazoTests(ITestOutputHelper output)
    {
        _output = output;
        _oEmbedProvider = new GyazoProvider();
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData("https://gyazo.com/c86f9566d5fd2904b2929ad4b67347c7")]
    [InlineData("http://gyazo.com/c86f9566d5fd2904b2929ad4b67347c7")]
    [InlineData("https://www.gyazo.com/c86f9566d5fd2904b2929ad4b67347c7")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task RequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Photo>("https://gyazo.com/c86f9566d5fd2904b2929ad4b67347c7");

        Assert.NotNull(result);
        Assert.Equal("photo", result!.Type);
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
        // Photo type values
        Assert.NotNull(result.Url);
        Assert.Equal(0, result.Width);
        Assert.Equal(0, result.Height);

        _output.WriteLine(result?.ProviderName);
        _output.WriteLine(result?.Url);
    }
}