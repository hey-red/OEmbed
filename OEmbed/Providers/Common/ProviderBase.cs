using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common
{
    public abstract record ProviderBase : IOEmbedProvider
    {
        private readonly List<string> _allowedHosts = new();

        private readonly Dictionary<IMatcher, ProviderScheme> _schemes = new();

        private IEnumerable<KeyValuePair<string, string?>> _parameters = Array.Empty<KeyValuePair<string, string?>>();

        protected void AddParameters(IEnumerable<KeyValuePair<string, string?>>? parameters)
        {
            if (parameters is not null)
            {
                _parameters = parameters;
            }
        }

        protected void AddAllowedHosts(IEnumerable<string> hosts) => _allowedHosts.AddRange(hosts);

        protected void AddScheme(IMatcher matcher, string apiEndpoint, ResourceType resourceType)
        {
            if (matcher is null)
            {
                throw new ArgumentNullException(nameof(matcher));
            }

            _schemes.Add(matcher, new(new Uri(apiEndpoint), resourceType));
        }

        public virtual bool CanProcess(Uri uri) => _allowedHosts.Contains(uri.Host);

        public virtual ResponseFormat ResponseType => ResponseFormat.Json;

        public IReadOnlyDictionary<IMatcher, ProviderScheme> Schemes => _schemes;

        public IEnumerable<KeyValuePair<string, string?>> Parameters => _parameters;
    }
}