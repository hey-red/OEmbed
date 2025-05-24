namespace OEmbed.Test;

public class BasicTests
{
    [Fact]
    public async Task SuccessVideoRequestTest()
    {
        var provider = new YoutubeProvider();
        OEmbedConsumer consumer = TestHelpers.BuildConsumer(new[] { provider });

        Base? result =
            await consumer.RequestAsync("https://music.youtube.com/watch?v=7nigXQS1Xb0&list=RDAMVM7nigXQS1Xb0");

        Assert.IsType<Video>(result);
    }

    [Fact]
    public async Task NotFoundRequestWithCacheTest()
    {
        var provider = new CoubProvider();
        OEmbedConsumer consumer = TestHelpers.BuildConsumer(new[] { provider }, true);

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            consumer.RequestAsync<Video>("https://coub.com/view/ut4ws6a3dsSfefsd"));
    }

    [Fact]
    public async Task NotFoundRequestWithoutCacheTest()
    {
        var provider = new CoubProvider();
        OEmbedConsumer consumer = TestHelpers.BuildConsumer(new[] { provider });

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            consumer.RequestAsync<Video>("https://coub.com/view/ut4ws6a3dsSfefsd"));
    }
}