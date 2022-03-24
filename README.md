# OEmbed

A simple [oEmbed](https://oembed.com) consumer library for .NET

## Install
via [NuGet](https://www.nuget.org/packages/OEmbed):
```
PM> Install-Package OEmbed
```

[DI extensions](https://www.nuget.org/packages/OEmbed.Extensions.Microsoft.DependencyInjection/) for Microsoft.Extensions.DependencyInjection:
```
PM> Install-Package OEmbed.Extensions.Microsoft.DependencyInjection
```

## DI configuration

```C#
services.AddOEmbed();

// or

services.AddOEmbed(options =>
{
    options.EnableCache = true; // true by default
});
```

By default it's register all providers listed below:

* CoubProvider
* DeviantartProvider
* FlickrProvider
* GfycatProvider
* GiphyProvider
* GyazoProvider
* ImgurProvider
* InstagramProvider
* KickstarterProvider
* PinterestProvider
* PixivProvider
* RedditProvider
* SoundcloudProvider
* SpotifyProvider
* TiktokProvider
* TumblrProvider
* TwitterProvider
* VimeoProvider
* YoutubeProvider

Additional providers can be added during configuration:

```C#
using HeyRed.OEmbed.Providers;

services.AddOEmbed()
    .ClearProviders() // remove all default providers
    .AddProvider<YoutubeProvider>()
    .AddProvider<VimeoProvider>()
    .Addprovider<ImgurProvider>();

// or with options
// NOTE: Some oembed providers defines additional parameters, so use "Parameters" option if you need them.
services.AddOEmbed()
    .ClearProviders() // remove all default providers
    .AddProvider<TwitterProvider>(options =>
    {
        options.Parameters = new Dictionary<string, string?>
        {
            ["theme"] = "dark"
        };
    })
    .AddProvider<FacebookProvider>(options =>
    {
        options.Parameters = new Dictionary<string, string?>
        {
            ["access_token"] = "app_id|token"
        };
    });
```

Additional providers:

* FacebookProvider
* AfreecatvProvider
* VliveProvider
* AnnieMusicProvider
* AudioboomProvider
* AudiomackProvider
* CodepenProvider
* YandexMusicProvider
* DeezerProvider
* DailymotionProvider

NOTE: While Instagram can work without access_token(with limited legacy endpoint), Facebook is just throw exception if you didn't provide these token.

## Usage

* Inject `IOEmbedConsumer` throught constructor injection.
* Call one of RequestAsync() overloads.

For example:
```C#
using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Models;

// Returns null if provider not found or HttpRequestException was thrown.
Video? result = await _oEmbedConsumer.RequestAsync<Video>("https://vimeo.com/22439234");
```
The result object is are similar to described [in the spec](https://oembed.com/#:~:text=2.3.4,parameters)

Models:
[Base](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Base.cs), [Link](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Link.cs), [Photo](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Photo.cs), [Rich](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Rich.cs), [Video](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Video.cs)

If you dont know which response models supported by provider, then use dynamic overload:
```C#
// Deserialize response based on provider preferences
dynamic? item = await _oEmbedConsumer.RequestAsync(url);

if (item is not null)
{
    if (item is Video) 
    {
        // work with video 
    }
    else if (item is Photo) 
    {
        // work with photo
    }
    else { //do something }
}
```

## Caching

Configure cache options:

```C#
services.AddOEmbed().Configure<CacheOptions>(options =>
{
    options.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(30); // Default is 1 hour
});
```

By default cache is enabled and it's default implementation is just a wrapper around [MemoryCache](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache)

You can write your own implementation of [ICache](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Abstractions/ICache.cs) and replace default cache during app configuration:
```C#
services.AddOEmbed().SetCache<DistributedRedisCache>();
```

## Additional providers

An easy way to write your own provider is inheritance of [ProviderBase](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Providers/Common/ProviderBase.cs) record:

```C#
public record ExampleProvider : ProviderBase
{
    // "ProviderOptions" is optional, you can safely remove argument from constructor
    public ExampleProvider(ProviderOptions? options = default)
    {
        AddParameters(options?.Parameters);
        
        // The Provider registry is primarily using this to select right provider at first check.
        // NOTE: Add all the hosts that will be used in the schemes below.
        AddAllowedHosts(new[] { "example.com", "www.example.com" });
        
        AddScheme(
            // Simple regex without hostname, "^" and "$" asserts. 
            // If this Regex is match string url, then scheme used to build request.
            matcher: new RegexMatcher(@"/\S+"),
            
            // API endpoint for current scheme
            apiEndpoint: "http://example.com/oembed",
            
            // The response type provided by resource.
            resourceType: ResourceType.Rich);
        }
    }
    
    // (Optional) Primary API response format(default is JSON)
    public override ResponseFormat ResponseType => ResponseFormat.Xml;
}
```

## License
[MIT](LICENSE)
