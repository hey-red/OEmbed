using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record YoutubeProvider : ProviderBase
    {
        public YoutubeProvider(ProviderOptions? options = default)
        {
            // I can't find documentation about additional parameters..
            AddParameters(options?.Parameters);

            AddAllowedHosts(new[]
            {
                "m.youtube.com",
                "youtu.be",
                "youtube.com",
                "www.youtu.be",
                "www.youtube.com"
            });

            AddScheme(
                matcher: new RegexMatcher(
                    @"(?:https?://)?(?:m\.)?(?:www\.)?
                    (?:youtu\.be\/|youtube\.com\/(?:embed\/|v\/|watch\?v=|watch\?.+&v=))
                    ([\w-]+)(?:(?:\?|&|\#)t=([a-z0-9]+))?(?:\S+)?"),
                apiEndpoint: "https://www.youtube.com/oembed",
                resourceType: ResourceType.Video);
        }
    }
}