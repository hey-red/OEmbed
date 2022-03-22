namespace OEmbed.Test
{
    public class BasicTests
    {
        [Fact]
        public async void NotFoundRequestWithCacheTest()
        {
            var provider = new CoubProvider();
            var consumer = TestHelpers.BuildConsumer(new[] { provider }, withCache: true);

            var result = await consumer.RequestAsync<Video>("https://coub.com/view/ut4ws6a3dsSfefsd");

            Assert.Null(result);
        }

        [Fact]
        public async void NotFoundRequestWithoutCacheTest()
        {
            var provider = new CoubProvider();
            var consumer = TestHelpers.BuildConsumer(new[] { provider }, withCache: false);

            var result = await consumer.RequestAsync<Video>("https://coub.com/view/ut4ws6a3dsSfefsd");

            Assert.Null(result);
        }
    }
}