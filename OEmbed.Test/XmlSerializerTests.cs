namespace OEmbed.Test
{
    public class XmlSerializerTests
    {
        private static readonly string _fixturesPath = TestHelpers.GetFixturesPath();

        [Fact]
        public void BasicTest()
        {
            var xmlSerializer = new DefaultXmlSerializer();

            using var fs = File.OpenRead(Path.Combine(_fixturesPath, "basic_sample.xml"));

            var result = xmlSerializer.Deserialize<Base>(fs);

            Assert.NotNull(result);
            Assert.Equal("link", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.Equal("Cal Henderson", result.AuthorName);
            Assert.Equal("http://iamcal.com/", result.AuthorUrl);
            Assert.Equal("iamcal.com", result.ProviderName);
            Assert.Equal("http://iamcal.com/", result.ProviderUrl);
            Assert.Equal("86400", result.CacheAge);
        }

        [Fact]
        public void RichTest()
        {
            var xmlSerializer = new DefaultXmlSerializer();

            using var fs = File.OpenRead(Path.Combine(_fixturesPath, "rich_sample.xml"));

            var result = xmlSerializer.Deserialize<Rich>(fs);

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.Equal("Test", result.Title);
            Assert.Equal("Test", result.AuthorName);
            Assert.Equal("https://test.com", result.AuthorUrl);
            Assert.Equal("Test", result.ProviderName);
            Assert.Equal("https://test.com", result.ProviderUrl);
            Assert.Equal(500, result.Width);
            Assert.Equal(500, result.Height);
            Assert.Equal("https://test/c", result.ThumbnailUrl);
            Assert.Equal(200, result.ThumbnailWidth);
            Assert.Equal(200, result.ThumbnailHeight);
            Assert.Equal("&lt;b&gt;awesome!&lt;/b&gt;", result.Html);
        }
    }
}