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

            //Regular video, channels, groups, ondemand
            AddScheme(
                matcher:
                @"(?:https?):\/\/(?:www\.)?vimeo\.com\/
                (?:channels\/(?:\w+\/)|groups\/(?:[^\/]*\/videos)\/|ondemand(.+)|)
                (\d+)?(?:|\/\?)(?:\?\S+)?",
                apiEndpoint: "https://vimeo.com/api/oembed.json",
                resourceType: ResourceType.Video);

            //https://vimeo.com/terjes/themountain
            //https://vimeo.com/terjes/themountain#t=5s
            AddScheme(
                matcher:
                @"(?:https?):\/\/(?:www\.)?vimeo\.com\/
                (?!ondemand)\w+\/(\w+)
                (?:(?:\?|\#)\S+)?",
                apiEndpoint: "https://vimeo.com/api/oembed.json",
                resourceType: ResourceType.Video);
        }
    }
}