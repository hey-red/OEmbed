namespace HeyRed.OEmbed.Models;

/// <summary>
///     This type is used for representing playable videos.
/// </summary>
public record Video : Base
{
    /// <summary>
    ///     The HTML required to embed a video player.
    ///     The HTML should have no padding or margins.
    ///     Consumers may wish to load the HTML in an off-domain iframe to avoid XSS vulnerabilities.
    /// </summary>
    /// <remarks>required</remarks>
    public string Html { get; init; } = default!;

    /// <summary>
    ///     The width in pixels required to display the HTML.
    /// </summary>
    /// <remarks>required</remarks>
    public int Width { get; init; }

    /// <summary>
    ///     The height in pixels required to display the HTML.
    /// </summary>
    /// <remarks>required</remarks>
    public int Height { get; init; }
}