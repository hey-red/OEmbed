namespace OEmbed.Test.CachingTests
{
    public class CacheKeyTests
    {
        private readonly ICacheKey _cacheKey;

        public CacheKeyTests() => _cacheKey = new DefaultCacheKey();

        [Fact]
        public void CreateKeyTest()
        {
            var key = _cacheKey.CreateKey("http://api.instagram.com/oembed?url=https%3A%2F%2Fwww.instagram.com%2Fp%2F1XSKgBAGz-%2F&format=json");

            Assert.Equal("7839ab3c5ae3506c4ebe2ec26010c4f3594abd5c0b0063deee9367bdbe3047cb", key);
        }

        [Theory]
        [InlineData("http://api.instagram.com/oembed?url=https%3A%2F%2Fwww.instagram.com%2Fp%2F1XSKgBAGz-%2F&format=json")]
        [InlineData("https://www.instagram.com/p/1XSKgBAGz-/")]
        public void KeyLengthTest(string url)
        {
            var key = _cacheKey.CreateKey(url);

            Assert.Equal(64, key.Length);
        }
    }
}