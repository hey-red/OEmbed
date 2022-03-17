using Microsoft.Extensions.DependencyInjection;

namespace HeyRed.OEmbed
{
    internal class OEmbedBuilder : IOEmbedBuilder
    {
        public OEmbedBuilder(IServiceCollection services) => Services = services;

        public IServiceCollection Services { get; }
    }
}