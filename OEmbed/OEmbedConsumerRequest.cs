using System;
using System.Net;
using System.Text;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed
{
    public record OEmbedConsumerRequest : IOEmbedConsumerRequest
    {
        public OEmbedConsumerRequest(
            Uri url,
            ResponseFormat? type = null,
            int? maxWidth = null,
            int? maxHeight = null)
        {
            Url = url.EnsureNotNull();
            Type = type;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
        }

        /// <summary>
        /// The URL to retrieve embedding information for.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// The required response format(XML/Json).
        /// </summary>
        public ResponseFormat? Type { get; }

        /// <summary>
        /// The maximum width of the embedded resource.
        /// </summary>
        public int? MaxWidth { get; }

        /// <summary>
        /// The maximum height of the embedded resource.
        /// </summary>
        public int? MaxHeight { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("?url=");
            sb.Append(WebUtility.UrlEncode(Url.OriginalString));

            if (MaxWidth is not null)
            {
                sb.Append("&maxwidth=");
                sb.Append(MaxWidth);
            }

            if (MaxHeight is not null)
            {
                sb.Append("&maxheight=");
                sb.Append(MaxHeight);
            }

            if (Type is not null)
            {
                sb.Append("&format=");
                sb.Append(Type.ToString()!.ToLower());
            }

            return sb.ToString();
        }
    }
}