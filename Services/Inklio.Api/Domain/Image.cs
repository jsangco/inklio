using Inklio.Api.SeedWork;

namespace Inklio.Api.Domain;

/// <summary>
/// Reperesents a image
/// </summary>
public class Image : Entity
{
    /// <summary>
    /// Gets or sets the UTC time the ask was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// Gets the user that created the comment.
    /// </summary>
    public User CreatedBy { get; private set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    public DateTime? EditedAtUtc { get; private set; }

    /// <summary>
    /// Gets or sets the UTC time the comment was last edited.
    /// </summary>
    public int? EditedById { get; private set; }

    /// <summary>
    /// Gets the content type of the image
    /// </summary>
    public string ContentType { get; private set; }

    /// <summary>
    /// Gets the name of the image
    /// </summary>
    public Guid Name { get; set; }

    /// <summary>
    /// Gets the size of the image in bytes
    /// </summary>
    public long SizeInBytes { get; private set; }

    /// <summary>
    /// Gets the ID of the parent Ask
    /// </summary>
    public int ThreadId { get; private set; }

    /// <summary>
    /// Gets the Ask associated with the image
    /// </summary>
    public Ask Thread { get; private set; }

    /// <summary>
    /// Gets the full URL for the image.
    /// </summary>
    public string? UrlFull { get; private set; }

    /// <summary>
    /// Gets the host of the URL for the image.
    /// </summary>
    public string? UrlHost { get; private set; }

    /// <summary>
    /// Gets the relative path of the URL for the image.
    /// </summary>
    public string? UrlRelative { get; private set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="Image"/> object.
    /// </summary>
#pragma warning disable CS8618
    protected Image()
    {
    }
#pragma warning restore CS8618

    /// <summary>
    /// Initilaizes an instance of a <see cref="Image"/>
    /// </summary>
    /// <param name="ask">The ask associated with the image</param>
    /// <param name="contentType">The contet type of the image</param>
    /// <param name="createdBy">The user that created the image</param>
    /// <param name="name">The name of the image</param>
    /// <param name="sizeInBytes">The size in bytes of the image</param>
    protected Image(Ask ask, User createdBy, Guid name, long sizeInBytes)
    {
        this.ContentType = string.Empty;
        this.CreatedAtUtc = DateTime.UtcNow;
        this.CreatedBy = createdBy;
        this.Name = name;
        this.ThreadId = ask.Id;
        this.Thread = ask;
    }

    /// <summary>
    /// Sets the url of the image.
    /// </summary>
    /// <param name="imageUrl">The url of the image to set</param>
    public void SetBlob(Blob blob)
    {
        this.ContentType = blob.ContentType;
        this.UrlFull = blob.Url.ToString();
        this.UrlHost = blob.Url.Host;
        this.UrlRelative = blob.Url.PathAndQuery;
    }
}