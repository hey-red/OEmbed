namespace OEmbed.Test
{
    public class BasicTests
    {
        [Fact]
        public async void NotFoundRequestWithCacheTest()
        {
            var provider = new CoubProvider();
            var consumer = TestHelpers.BuildConsumer(new[] { provider }, withCache: true);

            await Assert.ThrowsAsync<HttpRequestException>(() => consumer.RequestAsync<Video>("https://coub.com/view/ut4ws6a3dsSfefsd"));
        }

        [Fact]
        public async void NotFoundRequestWithoutCacheTest()
        {
            var provider = new CoubProvider();
            var consumer = TestHelpers.BuildConsumer(new[] { provider }, withCache: false);

            await Assert.ThrowsAsync<HttpRequestException>(() => consumer.RequestAsync<Video>("https://coub.com/view/ut4ws6a3dsSfefsd"));
        }
    }
}