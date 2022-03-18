using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common
{
    public abstract record ProviderBase : IOEmbedProvider
    {
        private readonly List<string> _allowedHosts = new();

        private readonly Dictionary<IUriMatcher, ProviderScheme> _schemes = new();

        private IEnumerable<KeyValuePair<string, string?>> _parameters = Array.Empty<KeyValuePair<string, string?>>();

        protected void AddParameters(IEnumerable<KeyValuePair<string, string?>>? parameters)
        {
            if (parameters is not null)
            {
                _parameters = parameters;
            }
        }

        protected void AddAllowedHosts(IEnumerable<string> hosts) => _allowedHosts.AddRange(hosts);

        protected void AddScheme(IUriMatcher matcher, string apiEndpoint, ResourceType resourceType)
        {
            _schemes.Add(matcher.EnsureNotNull(), new(new Uri(apiEndpoint), resourceType));
        }

        public virtual bool CanProcess(Uri uri) => _allowedHosts.Contains(uri.Host);

        public virtual ResponseFormat ResponseType => ResponseFormat.Json;

        public IReadOnlyDictionary<IUriMatcher, ProviderScheme> Schemes => _schemes;

        public IEnumerable<KeyValuePair<string, string?>> Parameters => _parameters;
    }
}