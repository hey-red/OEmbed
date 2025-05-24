using System.IO;

using HeyRed.OEmbed.Models;

namespace HeyRed.OEmbed.Abstractions;

public interface ISerializer
{
    T? Deserialize<T>(Stream content) where T : Base;
}