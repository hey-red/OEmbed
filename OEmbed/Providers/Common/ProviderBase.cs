﻿using System;
using System.Collections.Generic;
using System.Linq;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common;

public abstract record ProviderBase : IOEmbedProvider
{
    protected readonly List<string> _allowedHosts = new();

    protected readonly Dictionary<IUriMatcher, (Uri Endpoint, ResourceType ResourceType)> _schemes = new();

    protected IEnumerable<KeyValuePair<string, string?>> _parameters = Array.Empty<KeyValuePair<string, string?>>();

    public virtual bool CanProcess(Uri uri)
    {
        return _allowedHosts.Contains(uri.Host);
    }

    public virtual ProviderScheme? MatchScheme(Uri uri)
    {
        (Uri Endpoint, ResourceType ResourceType) pair = _schemes
            .Where(s => s.Key.IsMatch(uri))
            .Select(s => s.Value)
            .FirstOrDefault();

        if (pair != default)
        {
            return new ProviderScheme(pair.Endpoint, pair.ResourceType);
        }

        return null;
    }

    public virtual ResponseFormat Format
    {
        get => ResponseFormat.Json;
    }

    public IEnumerable<KeyValuePair<string, string?>> Parameters
    {
        get => _parameters;
    }

    public Func<Uri, Uri>? PreProcessUrl { get; set; }

    protected void AddParameters(IEnumerable<KeyValuePair<string, string?>>? parameters)
    {
        if (parameters is not null)
        {
            _parameters = parameters;
        }
    }

    protected void AddAllowedHosts(IEnumerable<string> hosts)
    {
        _allowedHosts.AddRange(hosts);
    }

    protected void AddScheme(
        IUriMatcher matcher,
        string apiEndpoint,
        ResourceType resourceType)
    {
        _schemes.Add(matcher.EnsureNotNull(), (new Uri(apiEndpoint), resourceType));
    }
}