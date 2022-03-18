using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common
{
    public abstract record ProviderBase : IOEmbedProvider
    {
        private readonly List<string> _allowedHosts = new();

        private readonly Dictionary<IMatcher, ProviderScheme> _schemes = new();

        protected void AddAllowedHosts(IEnumerable<string> hosts) => _allowedHosts.AddRange(hosts);

        protected void AddScheme(IMatcher matcher, string apiEndpoint, ResourceType resourceType)
        {
            _schemes.Add(matcher, new(new Uri(apiEndpoint), resourceType));
        }

        public virtual bool CanProcess(Uri uri) => _allowedHosts.Contains(uri.Host);

        public virtual ResponseFormat ResponseType => ResponseFormat.Json;

        public IDictionary<IMatcher, ProviderScheme> Schemes => _schemes;
    }
}