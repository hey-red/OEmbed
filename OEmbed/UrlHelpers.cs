using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace HeyRed.OEmbed
{
    public static class UrlHelpers
    {
        /// <summary>
        /// Append the given query keys and values to the URI.
        /// Extracted from Microsoft.AspNetCore.WebUtilities.QueryHelpers.
        /// </summary>
        /// <param name="uri">The base URI.</param>
        /// <param name="queryString">A collection of name value query pairs to append.</param>
        /// <returns>The combined result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="queryString"/> is <c>null</c>.</exception>
        public static string AddQueryString(
            string uri,
            IEnumerable<KeyValuePair<string, string?>> queryString)
        {
            uri.EnsureNotNull();
            queryString.EnsureNotNull();

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";
            // If there is an anchor, then the query string must be inserted before its first occurrence.
            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryString)
            {
                if (parameter.Value == null)
                {
                    continue;
                }

                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }
    }
}