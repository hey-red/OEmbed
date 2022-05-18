using System;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Defaults;
using HeyRed.OEmbed.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace HeyRed.OEmbed
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds OEmbed services to <see cref="IServiceCollection" /> with the default options.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IOEmbedBuilder"/></returns>
        public static IOEmbedBuilder AddOEmbed(this IServiceCollection services) => AddOEmbed(services, _ => { });

        /// <summary>
        /// Adds OEmbed services to <see cref="IServiceCollection" /> with the given options.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="setupAction"></param>
        /// <returns><see cref="IOEmbedBuilder"/></returns>
        public static IOEmbedBuilder AddOEmbed(this IServiceCollection services, Action<OEmbedOptions> setupAction)
        {
            var builder = new OEmbedBuilder(services);

            AddDefaultServices(builder, setupAction);

            builder.AddProvider<CoubProvider>();
            builder.AddProvider<DeviantartProvider>();
            builder.AddProvider<FlickrProvider>();
            builder.AddProvider<GfycatProvider>();
            builder.AddProvider<GiphyProvider>();
            builder.AddProvider<GyazoProvider>();
            builder.AddProvider<ImgurProvider>();
            builder.AddProvider<KickstarterProvider>();
            builder.AddProvider<PinterestProvider>();
            builder.AddProvider<PixivProvider>();
            builder.AddProvider<RedditProvider>();
            builder.AddProvider<SoundcloudProvider>();
            builder.AddProvider<SpotifyProvider>();
            builder.AddProvider<TiktokProvider>();
            builder.AddProvider<TumblrProvider>();
            builder.AddProvider<TwitterProvider>();
            builder.AddProvider<VimeoProvider>();
            builder.AddProvider<YoutubeProvider>();

            return builder;
        }

        private static void AddDefaultServices(IOEmbedBuilder builder, Action<OEmbedOptions> setupAction)
        {
            builder.Services.Configure(setupAction);

            builder.Services.AddSingleton<IJsonSerializer, DefaultJsonSerializer>();
            builder.Services.AddSingleton<IXmlSerializer, DefaultXmlSerializer>();

            builder.Services.AddSingleton<IProviderRegistry, ProviderRegistry>();

            builder.Services.AddHttpClient<IOEmbedConsumer, OEmbedConsumer>(httpClient =>
            {
                httpClient.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.SetCache<DefaultCache>();
            builder.SetCacheKey<DefaultCacheKey>();
        }
    }
}