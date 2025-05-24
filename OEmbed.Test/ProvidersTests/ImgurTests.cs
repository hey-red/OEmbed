namespace OEmbed.Test.ProvidersTests;

public class ImgurTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public ImgurTests(ITestOutputHelper output)
    {
        _output = output;
        _oEmbedProvider = new ImgurProvider();
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData("https://imgur.com/gallery/KTjpf")]
    [InlineData("https://i.imgur.com/aCRnwiC.jpeg")]
    [InlineData("https://imgur.com/t/iu/K7e7VSI")]
    [InlineData("https://i.stack.imgur.com/ELmEk.png")]
    [InlineData("https://imgur.com/a/IzHjJwJ")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Theory]
    [InlineData("https://i.imgur.com/aCRnwiC.jpegs")]
    [InlineData("https://s.stack.imgur.com/ELmEk.png")]
    [InlineData("https://s.imgur.com/aCRnwiC.jpeg")]
    [InlineData("https://imgur.com/z/IzHjJwJ")]
    public void UrlShouldNotMatchTest(string url)
    {
        TestHelpers.UrlShouldNotMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task RequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Rich>("https://i.imgur.com/aCRnwiC.jpeg");

        Assert.NotNull(result);
        Assert.Equal("rich", result!.Type);
        Assert.Equal("1.0", result.Version);
        Assert.Null(result.Title);
        Assert.Null(result.AuthorName);
        Assert.Null(result.AuthorUrl);
        Assert.NotNull(result.ProviderName);
        Assert.NotNull(result.ProviderUrl);
        Assert.Null(result.CacheAge);
        Assert.Null(result.ThumbnailUrl);
        Assert.Null(result.ThumbnailWidth);
        Assert.Null(result.ThumbnailHeight);
        // Rich type values
        Assert.NotNull(result.Html);
        Assert.NotEqual(0, result.Width);
        Assert.NotEqual(0, result.Height);

        _output.WriteLine(result?.ProviderName);
        _output.WriteLine(result?.ProviderUrl);
    }
}