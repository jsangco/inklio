using Azure.Storage.Blobs;

namespace Inklio.Api.Domain;

/// <summary>
/// An interface for a <see cref="Image"/> repository
/// </summary>
public interface IBlobRepository
{
    Task<Uri> AddAskImageAsync(string contentType, IFormFile formFile, Guid name, CancellationToken cancellationToken);

    Task DeleteAskImageAsync(Guid name, CancellationToken cancellationToken);
}