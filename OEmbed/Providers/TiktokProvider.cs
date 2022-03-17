namespace HeyRed.OEmbed.Providers
{
    public record TiktokProvider : ProviderBase
    {
        public TiktokProvider()
        {
            AddAllowedHosts(new[]
            {
                "tiktok.com",
                "www.tiktok.com"
            });

            AddScheme(
                matcher: @"https?://(?:www\.)?tiktok\.com/\S+",
                apiEndpoint: "https://www.tiktok.com/oembed",
                resourceType: ResourceType.Video);
        }
    }
}