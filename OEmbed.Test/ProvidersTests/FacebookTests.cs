using HeyRed.OEmbed.Providers.Common;

using Microsoft.Extensions.Configuration;

namespace OEmbed.Test.ProvidersTests;

public class FacebookTests
{
    private readonly IOEmbedConsumer _oEmbedConsumer;

    private readonly IOEmbedProvider _oEmbedProvider;
    private readonly ITestOutputHelper _output;

    public FacebookTests(ITestOutputHelper output)
    {
        _output = output;

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddUserSecrets<Secrets>()
            .Build();

        _oEmbedProvider = new FacebookProvider(new ProviderOptions
        {
            Parameters = new Dictionary<string, string?>
            {
                ["access_token"] = configuration["MetaAccessToken"]
            }
        });
        _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
    }

    [Theory]
    [InlineData("https://www.facebook.com/iu.loen/posts/501755841318628")]
    [InlineData("https://www.facebook.com/iu.loen/photos/281842976643250")]
    [InlineData("https://www.facebook.com/photo.php?fbid=281842976643250&set=pb.100044526485652.-2207520000..&type=3")]
    [InlineData("https://www.facebook.com/photo/?fbid=3613461915406206&set=a.277059300454951")]
    [InlineData("https://www.facebook.com/iu.loen/videos/537284977236313")]
    [InlineData("https://www.facebook.com/iu.loen")]
    public void UrlMatchTest(string url)
    {
        TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
    }

    [Fact]
    public async Task PostRequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Rich>("https://www.facebook.com/iu.loen/posts/501755841318628");

        Assert.NotNull(result);
        Assert.Equal("rich", result!.Type);
        Assert.Equal("1.0", result.Version);
        Assert.Null(result.Title);
        Assert.NotNull(result.AuthorName);
        Assert.NotNull(result.AuthorUrl);
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

        _output.WriteLine(result?.AuthorName);
        _output.WriteLine(result?.AuthorUrl);
    }

    [Fact]
    public async Task VideoRequestTest()
    {
        var result =
            await _oEmbedConsumer.RequestAsync<Video>("https://www.facebook.com/iu.loen/videos/537284977236313");

        Assert.NotNull(result);
        Assert.Equal("video", result!.Type);
        Assert.Equal("1.0", result.Version);
        Assert.Null(result.Title);
        Assert.NotNull(result.AuthorName);
        Assert.NotNull(result.AuthorUrl);
        Assert.NotNull(result.ProviderName);
        Assert.NotNull(result.ProviderUrl);
        Assert.Null(result.CacheAge);
        Assert.Null(result.ThumbnailUrl);
        Assert.Null(result.ThumbnailWidth);
        Assert.Null(result.ThumbnailHeight);
        // Video type values
        Assert.NotNull(result.Html);
        Assert.NotEqual(0, result.Width);
        Assert.NotEqual(0, result.Height);

        _output.WriteLine(result?.AuthorName);
        _output.WriteLine(result?.AuthorUrl);
    }

    [Fact]
    public async Task PageRequestTest()
    {
        var result = await _oEmbedConsumer.RequestAsync<Rich>("https://www.facebook.com/iu.loen");

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