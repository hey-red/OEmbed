namespace HeyRed.OEmbed.Models;

/// <summary>
///     Base class for oEmbed responses
/// </summary>
public record Base
{
    /// <summary>
    ///     The resource type.
    /// </summary>
    /// <remarks>required</remarks>
    public string Type { get; init; } = default!;

    /// <summary>
    ///     The oEmbed version number. This must be 1.0.
    /// </summary>
    /// <remarks>required</remarks>
    public string Version { get; init; } = default!;

    /// <summary>
    ///     A text title, describing the resource.
    /// </summary>
    /// <remarks>optional</remarks>
    public string? Title { get; init; }

    /// <summary>
    ///     The name of the author/owner of the resource.
    /// </summary>
    /// <remarks>optional</remarks>
    public string? AuthorName { get; init; }

    /// <summary>
    ///     A URL for the author/owner of the resource.
    /// </summary>
    /// <remarks>optional</remarks>
    public string? AuthorUrl { get; init; }

    /// <summary>
    ///     The name of the resource provider.
    /// </summary>
    /// <remarks>optional</remarks>
    public string? ProviderName { get; init; }

    /// <summary>
    ///     The url of the resource provider.
    /// </summary>
    /// <remarks>optional</remarks>
    public string? ProviderUrl { get; init; }

    /// <summary>
    ///     The suggested cache lifetime for this resource, in seconds.
    ///     Consumers may choose to use this value or not.
    /// </summary>
    /// <remarks>optional</remarks>
    public string? CacheAge { get; init; }

    /// <summary>
    ///     A URL to a thumbnail image representing the resource.
    ///     The thumbnail must respect any maxwidth and maxheight parameters.
    ///     If this parameter is present, thumbnail_width and thumbnail_height must also be present.
    /// </summary>
    /// <remarks>optional</remarks>
    public string? ThumbnailUrl { get; init; }

    /// <summary>
    ///     The width of the optional thumbnail.
    ///     If this parameter is present, thumbnail_url and thumbnail_height must also be present.
    /// </summary>
    /// <remarks>optional</remarks>
    public int? ThumbnailWidth { get; init; }

    /// <summary>
    ///     The height of the optional thumbnail.
    ///     If this parameter is present, thumbnail_url and thumbnail_width must also be present.
    /// </summary>
    /// <remarks>optional</remarks>
    public int? ThumbnailHeight { get; init; }
}