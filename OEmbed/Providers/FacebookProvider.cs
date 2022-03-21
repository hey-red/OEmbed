﻿using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record FacebookProvider : ProviderBase
    {
        public FacebookProvider(ProviderOptions options)
        {
            options.Parameters.ThrowIfInvalidMetaAccessToken();

            AddParameters(options.Parameters);

            AddAllowedHosts(new[] { "facebook.com", "www.facebook.com" });

            // POST
            /*
             * https://www.facebook.com/{page-name}/posts/{post-id}
             * https://www.facebook.com/{username}/posts/{post-id}
             * https://www.facebook.com/{username}/activity/{activity-id}
             * https://www.facebook.com/{username}/photos/{photo-id}
             */
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?facebook\.com/(?:[^/]+)/(?:posts|activity|photos)/([^/]+)/?"),
                apiEndpoint: "https://graph.facebook.com/v13.0/oembed_post",
                resourceType: ResourceType.Rich);

            // https://www.facebook.com/notes/{username}/{note-url}/{note-id}
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?facebook\.com/notes/(?:[^/]+)/(?:[^/]+)/(?:[^/]+)/?"),
                apiEndpoint: "https://graph.facebook.com/v13.0/oembed_post",
                resourceType: ResourceType.Rich);

            /*
             * https://www.facebook.com/photo.php?fbid={photo-id}
             * https://www.facebook.com/photo/?fbid={photo-id}
             * https://www.facebook.com/permalink.php?story_fbid={post-id}&id={page-or-user-id}
             */
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?facebook\.com/(?:photo|permalink)(?:\.php|/)\?(?:(?:story_)?fbid)=([0-9]+)\S*"),
                apiEndpoint: "https://graph.facebook.com/v13.0/oembed_post",
                resourceType: ResourceType.Rich);

            /*
             * https://www.facebook.com/photos/{photo-id}
             * https://www.facebook.com/questions/{question-id}
             * https://www.facebook.com/media/set?set={set-id}
             */
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?facebook\.com/(?:photos/|questions/|media/set\?set=)(?:[^/]+)/?"),
                apiEndpoint: "https://graph.facebook.com/v13.0/oembed_post",
                resourceType: ResourceType.Rich);

            // VIDEO
            /*
             * https://www.facebook.com/{page-name}/videos/{video-id}/
             * https://www.facebook.com/{username}/videos/{video-id}/
             */
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?facebook\.com/(?:[^/]+)/videos/([\d]+)/?"),
                apiEndpoint: "https://graph.facebook.com/v13.0/oembed_video",
                resourceType: ResourceType.Video);

            /*
             * https://www.facebook.com/video.php?id={video-id}
             * https://www.facebook.com/video.php?v={video-id}
             */
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?facebook\.com/video\.php\?(?:id|v)=([\d]+)/?"),
                apiEndpoint: "https://graph.facebook.com/v13.0/oembed_video",
                resourceType: ResourceType.Video);

            // PAGE
            /*
             * https://www.facebook.com/{page-name}
             * https://www.facebook.com/{page-id}
             */
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?facebook\.com\/([a-zA-Z0-9\.-]+)/?"),
                apiEndpoint: "https://graph.facebook.com/v13.0/oembed_page",
                resourceType: ResourceType.Rich);
        }
    }
}