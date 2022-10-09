namespace Inklio.Api.Domain;

/// <summary>
/// An interface for a <see cref="Image"/> repository
/// </summary>
public interface IBlobRepository
{
    /// <summary>
    /// Adds a formfile to the Ask container.
    /// </summary>
    /// <param name="formFile">The web form file to upload</param>
    /// <param name="name">The name for the blob</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The blob uri of the stored image.</returns>
    Task<Blob> AddAskBlobAsync(IFormFile formFile, Guid name, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a formfile to the Delivery container.
    /// </summary>
    /// <param name="formFile">The web form file to upload</param>
    /// <param name="name">The name for the blob</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The blob uri of the stored blob.</returns>
    Task<Blob> AddDeliveryBlobAsync(IFormFile formFile, Guid name, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a blob from the Ask container.
    /// </summary>
    /// <param name="name">The name of the blob to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A task</returns>
    Task DeleteAskBlobAsync(Guid name, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a blob from the Delivery container.
    /// </summary>
    /// <param name="name">The name of the blob to delete</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A task</returns>
    Task DeleteDeliveryBlobAsync(Guid name, CancellationToken cancellationToken);
}