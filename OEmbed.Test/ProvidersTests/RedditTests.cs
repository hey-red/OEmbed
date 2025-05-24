namespace OEmbed.Test.ProvidersTests;

public class RedditTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public RedditTests(ITestOutputHelper output)
    {
        _output = output;
        _oEmbedProvider = new RedditProvider();
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData(
        "https://www.reddit.com/r/SoulsSliders/comments/t7bsvp/someone_requested_kristen_ritter_figured_id_post/")]
    [InlineData(
        "https://www.reddit.com/r/SoulsSliders/comments/t7bsvp/someone_requested_kristen_ritter_figured_id_post/?utm_source=share&utm_medium=web2x&context=3")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task RequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Rich>(
            "https://www.reddit.com/r/todayilearned/comments/1fzx2x3/til_that_heath_ledger_refused_to_present_the/");

        Assert.NotNull(result);
        Assert.Equal("rich", result!.Type);
        //Assert.Equal("1.0", result.Version);
        Assert.NotNull(result.Title);
        Assert.NotNull(result.AuthorName);
        Assert.Null(result.AuthorUrl);
        Assert.Equal("reddit", result.ProviderName);
        Assert.Equal("https://www.reddit.com", result.ProviderUrl);
        Assert.Null(result.CacheAge);
        Assert.Null(result.ThumbnailUrl);
        Assert.Null(result.ThumbnailWidth);
        Assert.Null(result.ThumbnailHeight);
        // Rich type values
        Assert.NotNull(result.Html);
        Assert.Null(result.Width);
        Assert.NotNull(result.Height);

        _output.WriteLine(result?.AuthorName);
        _output.WriteLine(result?.Title);
    }
}