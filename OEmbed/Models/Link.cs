namespace HeyRed.OEmbed.Models
{
    /// <summary>
    /// Responses of this type allow a provider to return any generic embed data (such as title and author_name),
    /// without providing either the url or html parameters.
    /// The consumer may then link to the resource, using the URL specified in the original request.
    /// </summary>
    public record Link : Base
    {
    }
}