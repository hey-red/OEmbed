namespace HeyRed.OEmbed.Abstractions;

public interface ICacheKey
{
    string CreateKey(string url);
}