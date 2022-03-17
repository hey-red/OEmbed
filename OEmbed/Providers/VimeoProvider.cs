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
                matcher:
                @"(?:https?):\/\/(?:www\.)?vimeo\.com\/
                (?:channels\/(?:\w+\/)?|groups\/(?:[^\/]*\/videos)\/|ondemand(.+)|)
                (\d+)?(?:|\/\?)(?:\?\S+)?",
                apiEndpoint: "https://vimeo.com/api/oembed.json",
                resourceType: ResourceType.Video);
        }
    }
}