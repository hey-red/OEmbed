namespace OEmbed.Test.CachingTests
{
    public class CacheTests
    {
        private readonly ICacheKey _cacheKey;

        private readonly ITestOutputHelper _output;

        public CacheTests(ITestOutputHelper outputHelper)
        {
            _cacheKey = new DefaultCacheKey();
            _output = outputHelper;
        }

        [Fact]
        public async void AddOrGetExistingAsyncTest()
        {
            var cache = new DefaultCache();

            string url = "https://www.youtube.com/watch?v=D1PvIWdJ8xo";
            var expected = new Base { AuthorName = "IU", Title = "blueming" };

            var key = _cacheKey.CreateKey(url);

            var item = await cache.AddOrGetExistingAsync(url, (url) => Task.FromResult(expected));

            var cachedItem = await cache.GetAsync<Base>(key);

            Assert.NotNull(item);
            Assert.NotNull(cachedItem);

            Assert.Equal(expected, item);
            Assert.Equal(expected, cachedItem);
        }

        [Fact]
        public async void MultiThreadingTest()
        {
            var cache = new DefaultCache();

            int requestsCounter = 0;

            // List of dublicated links
            var urls = Enumerable.Repeat("https://www.youtube.com/watch?v=D1PvIWdJ8xo", 20).ToList();

            urls.Add("https://www.instagram.com/p/1XSKgBAGz-/");
            urls.Add("https://vimeo.com/22439234");

            await Task.WhenAll(urls.Select(async url =>
            {
                return await cache.AddOrGetExistingAsync(url, async (url) =>
                {
                    _output.WriteLine("Request #" + ++requestsCounter);

                    await Task.Delay(3000); // simulate long running request

                    return new Base();
                });
            }));

            // Should be 3, because we use keylock to prevent race condition
            Assert.Equal(3, requestsCounter);
        }
    }
}