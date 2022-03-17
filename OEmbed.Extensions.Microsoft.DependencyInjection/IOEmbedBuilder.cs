using Microsoft.Extensions.DependencyInjection;

namespace HeyRed.OEmbed
{
    public interface IOEmbedBuilder
    {
        IServiceCollection Services { get; }
    }
}