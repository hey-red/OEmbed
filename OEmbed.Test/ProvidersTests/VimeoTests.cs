﻿namespace OEmbed.Test.ProvidersTests;

public class VimeoTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public VimeoTests(ITestOutputHelper output)
    {
        _output = output;
        _oEmbedProvider = new VimeoProvider();
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData("https://vimeo.com/22439234")]
    [InlineData("https://vimeo.com/22439234?embedded=true&source=video_title&owner=910279")]
    [InlineData("https://vimeo.com/groups/musicvideo/videos/306797894")]
    [InlineData("https://vimeo.com/channels/music/244199526")]
    [InlineData("https://vimeo.com/ondemand/sydneyrenaealbumrelease")]
    [InlineData("https://vimeo.com/terjes/themountain")]
    [InlineData("https://vimeo.com/terjes/themountain#t=5s")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task RequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Video>("https://vimeo.com/22439234");

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