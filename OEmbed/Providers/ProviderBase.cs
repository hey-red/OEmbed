using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers
{
    public abstract record ProviderBase : IOEmbedProvider
    {
        private readonly List<string> _allowedHosts = new();

        private readonly Dictionary<Regex, ProviderScheme> _schemes = new();

        protected void AddAllowedHosts(IEnumerable<string> hosts) => _allowedHosts.AddRange(hosts);

        protected void AddScheme(string matcher, string apiEndpoint, ResourceType resourceType)
        {
            _schemes.Add(new("^" + matcher + "$", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled), new(new Uri(apiEndpoint), resourceType));
        }

        public virtual bool CanProcess(Uri uri) => _allowedHosts.Contains(uri.Host);

        public virtual ResponseFormat ResponseType => ResponseFormat.Json;

        public IDictionary<Regex, ProviderScheme> Schemes => _schemes;
    }
}