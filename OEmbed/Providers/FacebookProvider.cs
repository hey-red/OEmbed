﻿using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record FacebookProvider : ProviderBase
{
    public FacebookProvider(ProviderOptions options)
    {
        options.Parameters.ThrowIfInvalidMetaAccessToken();

        AddParameters(options.Parameters);

        AddAllowedHosts(new[] { "facebook.com", "www.facebook.com" });

        // POST
        AddScheme(
            new RegexMatcher(
                /*
                 * https://www.facebook.com/{page-name}/posts/{post-id}
                 * https://www.facebook.com/{username}/posts/{post-id}
                 * https://www.facebook.com/{username}/activity/{activity-id}
                 * https://www.facebook.com/{username}/photos/{photo-id}
                 */
                @"/(?:[^/]+)/(?:posts|activity|photos)/([^/]+)/?",

                // https://www.facebook.com/notes/{username}/{note-url}/{note-id}
                @"/notes/(?:[^/]+)/(?:[^/]+)/(?:[^/]+)/?",
                /*
                 * https://www.facebook.com/photo.php?fbid={photo-id}
                 * https://www.facebook.com/photo/?fbid={photo-id}
                 * https://www.facebook.com/permalink.php?story_fbid={post-id}&id={page-or-user-id}
                 */
                @"/(?:photo|permalink)(?:\.php|/)\?(?:(?:story_)?fbid)=([0-9]+)\S*",
                /*
                 * https://www.facebook.com/photos/{photo-id}
                 * https://www.facebook.com/questions/{question-id}
                 * https://www.facebook.com/media/set?set={set-id}
                 */
                @"/(?:photos/|questions/|media/set\?set=)(?:[^/]+)/?"),
            "https://graph.facebook.com/v18.0/oembed_post",
            ResourceType.Rich);

        // VIDEO
        AddScheme(
            new RegexMatcher(
                /*
                 * https://www.facebook.com/{page-name}/videos/{video-id}/
                 * https://www.facebook.com/{username}/videos/{video-id}/
                 */
                @"/(?:[^/]+)/videos/([\d]+)/?",
                /*
                 * https://www.facebook.com/video.php?id={video-id}
                 * https://www.facebook.com/video.php?v={video-id}
                 */
                @"/video\.php\?(?:id|v)=([\d]+)/?"),
            "https://graph.facebook.com/v18.0/oembed_video",
            ResourceType.Video);

        /*
         * PAGE
         * https://www.facebook.com/{page-name}
         * https://www.facebook.com/{page-id}
         */
        AddScheme(
            new RegexMatcher(@"/([a-zA-Z0-9\.-]+)/?"),
            "https://graph.facebook.com/v18.0/oembed_page",
            ResourceType.Rich);
    }
}