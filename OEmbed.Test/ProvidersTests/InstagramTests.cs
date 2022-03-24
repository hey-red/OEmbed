﻿using Microsoft.Extensions.Configuration;

namespace OEmbed.Test.ProvidersTests
{
    public class InstagramTests
    {
        private readonly ITestOutputHelper _output;

        private readonly IOEmbedProvider _legacyOembedProvider;

        private readonly IOEmbedConsumer _legacyOembedConsumer;

        private readonly IOEmbedProvider _oEmbedProvider;

        private readonly IOEmbedConsumer _oEmbedConsumer;

        public InstagramTests(ITestOutputHelper output)
        {
            _output = output;

            _legacyOembedProvider = new InstagramProvider();
            _legacyOembedConsumer = TestHelpers.BuildConsumer(new[] { _legacyOembedProvider });

            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Secrets>()
                .Build();

            _oEmbedProvider = new InstagramProvider(new()
            {
                Parameters = new Dictionary<string, string?>
                {
                    ["access_token"] = configuration["MetaAccessToken"]
                }
            });
            _oEmbedConsumer = TestHelpers.BuildConsumer(new[] { _oEmbedProvider });
        }

        [Theory]
        [InlineData("https://www.instagram.com/dlwlrma/")]
        [InlineData("https://www.instagram.com/p/1XSKgBAGz-/")]
        [InlineData("https://www.instagram.com/tv/CHLVnWVAF9I/")]
        [InlineData("https://www.instagram.com/reel/CW0gZu2rouF/")]
        [InlineData("https://www.instagr.am/dlwlrma/p/1XSKgBAGz-/")]
        [InlineData("https://www.instagr.am/dlwlrma/tv/CbKjea_ARNB/")]
        [InlineData("https://www.instagram.com/dlwlrma/tv/CbKjea_ARNB/")]
        [InlineData("https://www.instagram.com/p/1XSKgBAGz-/?utm_source=ig_web_button_share_sheet")]
        public void UrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_oEmbedProvider, url);
        }

        [Fact]
        public async void RequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://www.instagr.am/dlwlrma/tv/CbKjea_ARNB/");

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.Null(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.Null(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);
            Assert.Null(result.CacheAge);
            Assert.NotNull(result.ThumbnailUrl);
            Assert.NotNull(result.ThumbnailWidth);
            Assert.NotNull(result.ThumbnailHeight);
            // Rich type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.Null(result.Height);

            _output.WriteLine(result?.AuthorName);
        }

        [Fact]
        public async void ProfileRequestTest()
        {
            var result = await _oEmbedConsumer.RequestAsync<Rich>("https://www.instagram.com/dlwlrma/");

            Assert.NotNull(result);
            Assert.Equal("rich", result!.Type);
            Assert.Equal("1.0", result.Version);
            Assert.Null(result.Title);
            Assert.NotNull(result.AuthorName);
            Assert.Null(result.AuthorUrl);
            Assert.NotNull(result.ProviderName);
            Assert.NotNull(result.ProviderUrl);
            Assert.Null(result.CacheAge);
            Assert.NotNull(result.ThumbnailUrl);
            Assert.NotNull(result.ThumbnailWidth);
            Assert.NotNull(result.ThumbnailHeight);
            // Rich type values
            Assert.NotNull(result.Html);
            Assert.NotEqual(0, result.Width);
            Assert.Null(result.Height);

            _output.WriteLine(result?.AuthorName);
        }

        [Theory]
        [InlineData("https://instagram.com/p/1XSKgBAGz-")]
        [InlineData("https://www.instagram.com/p/1XSKgBAGz-/")]
        [InlineData("https://www.instagr.am/p/1XSKgBAGz-/")]
        [InlineData("https://www.instagram.com/p/1XSKgBAGz-/?utm_source=ig_web_button_share_sheet")]
        public void LegacySchemeUrlMatchTest(string url)
        {
            TestHelpers.UrlShouldMatchTest(_legacyOembedProvider, url);
        }

        [Fact]
        public async void LegacySchemeRequestTest()
        {
            var result = await _legacyOembedConsumer.RequestAsync<Rich>("https://www.instagram.com/p/1XSKgBAGz-/");

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
}