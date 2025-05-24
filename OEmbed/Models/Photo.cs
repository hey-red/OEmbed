namespace HeyRed.OEmbed.Models;

/// <summary>
///     This type is used for representing static photos.
/// </summary>
public record Photo : Base
{
    /// <summary>
    ///     The source URL of the image.
    ///     Consumers should be able to insert this URL into an
    ///     <img>
    ///         element.
    ///         Only HTTP and HTTPS URLs are valid.
    /// </summary>
    /// <remarks>required</remarks>
    public string Url { get; init; } = default!;

    /// <summary>
    ///     The width in pixels of the image specified in the url parameter.
    /// </summary>
    /// <remarks>required</remarks>
    public int Width { get; init; }

    /// <summary>
    ///     The height in pixels of the image specified in the url parameter.
    /// </summary>
    /// <remarks>required</remarks>
    public int Height { get; init; }
}