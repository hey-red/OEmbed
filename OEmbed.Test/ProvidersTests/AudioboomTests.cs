﻿namespace OEmbed.Test.ProvidersTests;

public class AudioboomTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public AudioboomTests(ITestOutputHelper output)
    {
        _output = output;
        _oEmbedProvider = new AudioboomProvider();
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData("https://audioboom.com/channel/devils")]
    [InlineData("https://audioboom.com/posts/8048263-tracie-andrews-the-femme-fatale-killer-who-tricked-the-nation")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task RequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Rich>("https://audioboom.com/channel/devils");

        Assert.NotNull(result);
        Assert.Equal("rich", result!.Type);
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
        // Rich type values
        Assert.NotNull(result.Html);
        Assert.NotEqual(0, result.Width);
        Assert.NotEqual(0, result.Height);

        _output.WriteLine(result?.AuthorName);
        _output.WriteLine(result?.Title);
    }
}