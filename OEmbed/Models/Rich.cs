namespace HeyRed.OEmbed.Models;

/// <summary>
///     This type is used for rich HTML content that does not fall under one of the other categories.
///     NOTE: Some providers(like twitter/instagram) does not provide a value for <see cref="Height" />, so I just make w/h
///     nullable.
/// </summary>
public record Rich : Base
{
    /// <summary>
    ///     The HTML required to display the resource.
    ///     The HTML should have no padding or margins.
    ///     Consumers may wish to load the HTML in an off-domain iframe to avoid XSS vulnerabilities.
    ///     The markup should be valid XHTML 1.0 Basic.
    /// </summary>
    /// <remarks>required</remarks>
    public string Html { get; init; } = default!;

    /// <summary>
    ///     The width in pixels required to display the HTML.
    /// </summary>
    /// <remarks>required</remarks>
    public int? Width { get; init; }

    /// <summary>
    ///     The height in pixels required to display the HTML.
    /// </summary>
    /// <remarks>required</remarks>
    public int? Height { get; init; }
}