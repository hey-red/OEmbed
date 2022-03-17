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

By default it's register all built in providers:

* InstagramProvider
* TiktokProvider
* TwitterProvider
* VimeoProvider
* YoutubeProvider

You can add a provider during configuration:

```C#
services.AddOEmbed()
  .ClearProviders() // remove all default providers
  .AddProvider<YoutubeProvider>()
  .AddProvider<VimeoProvider>();
```

## Usage

* Inject `IOEmbedConsumer` throught constructor injection.
* Call one of RequestAsync() overloads.

For example:
```C#
using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Models;

// Returns null if provider not found for given url
// NOTE: This method can throw HttpRequestException, so wrap your request with try/catch if it needed
Video? result = await _oEmbedConsumer.RequestAsync<Video>("https://vimeo.com/22439234");
```
The result object is are similar to described [in the spec](https://oembed.com/#:~:text=2.3.4,parameters)

Models:
[Base](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Base.cs), [Link](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Link.cs), [Photo](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Photo.cs), [Rich](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Rich.cs), [Video](https://github.com/hey-red/OEmbed/blob/master/OEmbed/Models/Video.cs)

If you dont know which response models supported by provider, then use dynamic overload.
```C#
// Deserialize response based on provider preferences
dynamic? item = await _oEmbedConsumer.RequestAsync(url);

if (item is not null)
{
  if (item is Video) 
  { 
    // work with video 
  }
  elseif (item is Photo) 
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

## License
[MIT](LICENSE)
