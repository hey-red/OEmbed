namespace HeyRed.OEmbed.Providers
{
    public record YoutubeProvider : ProviderBase
    {
        public YoutubeProvider()
        {
            AddAllowedHosts(new[]
            {
                "m.youtube.com",
                "youtu.be",
                "youtube.com",
                "www.youtu.be",
                "www.youtube.com"
            });

            AddScheme(
                matcher: @"(?:https?:\/\/)?(?:m\.)?(?:www\.)?
                           (?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))
                           ([\w-]+)(?:(?:\?|&|\#)t=([a-z0-9]+))?(?:\S+)?",
                apiEndpoint: "https://www.youtube.com/oembed",
                resourceType: ResourceType.Video);
        }
    }
}