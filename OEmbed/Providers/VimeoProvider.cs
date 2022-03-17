namespace HeyRed.OEmbed.Providers
{
    public record VimeoProvider : ProviderBase
    {
        public VimeoProvider()
        {
            AddAllowedHosts(new[]
            {
                "vimeo.com",
                "player.vimeo.com"
            });

            AddScheme(
                matcher: @"https?://(?:player\.)?vimeo\.com/\S+",
                apiEndpoint: "https://vimeo.com/api/oembed.json",
                resourceType: ResourceType.Video);
        }
    }
}