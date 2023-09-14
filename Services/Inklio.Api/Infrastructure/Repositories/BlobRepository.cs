using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Inklio.Api.Domain;

namespace Inklio.Api.Infrastructure.Repositories;

public class BlobRepository : IBlobRepository
{
    private const string AskBlobBlobContainerName = "asks";
    private const string DeliveryBlobBlobContainerName = "deliveries";
    private readonly BlobServiceClient blobServiceClient;
    private BlobContainerClient askContainer;
    private BlobContainerClient deliveryContainer;

    /// <summary>
    /// Initialize of a new instance of a <see cref="BlobRepository"/> object
    /// </summary>
    /// <param name="context">The db context.</param>
    public BlobRepository(BlobServiceClient blobServiceClient)
    {
        this.blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
        this.askContainer = blobServiceClient.GetBlobContainerClient(AskBlobBlobContainerName);
        this.deliveryContainer = blobServiceClient.GetBlobContainerClient(DeliveryBlobBlobContainerName);

        this.askContainer.CreateIfNotExists(PublicAccessType.Blob);
        this.deliveryContainer.CreateIfNotExists(PublicAccessType.Blob);
    }

    /// <inheritdoc/>
    public Task<Blob> AddAskBlobAsync(IFormFile formFile, Guid name, CancellationToken cancellationToken)
    {
        return this.AddBlobAsync(this.askContainer, formFile, name, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<Blob> AddDeliveryBlobAsync(IFormFile formFile, Guid name, CancellationToken cancellationToken)
    {
        return this.AddBlobAsync(this.deliveryContainer, formFile, name, cancellationToken);
    }

    /// <inheritdoc/>
    public Task DeleteAskBlobAsync(Guid name, CancellationToken cancellationToken)
    {
        return this.DeleteBlobAsync(this.askContainer, name, cancellationToken);
    }

    /// <inheritdoc/>
    public Task DeleteDeliveryBlobAsync(Guid name, CancellationToken cancellationToken)
    {
        return this.DeleteBlobAsync(this.deliveryContainer, name, cancellationToken);
    }

    /// <summary>
    /// Adds a formfile to a blob container.
    /// </summary>
    /// <param name="blobContainer">The blob container to upload to</param>
    /// <param name="formFile">The form file to upload</param>
    /// <param name="name">The name of the file to upload</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>The blob uri of the stored image.</returns>
    private async Task<Blob> AddBlobAsync(BlobContainerClient blobContainer, IFormFile formFile, Guid name, CancellationToken cancellationToken)
    {
        // Read file header and determine content type
        using Stream readStream = formFile.OpenReadStream();
        (byte[] fileHeader, string contentType) = this.ReadAndValidateFileHeader(readStream);

        // Create blob
        string blobName = name.ToString() + this.GetFileExtensionFromContentType(contentType);
        var blobClient = blobContainer.GetBlobClient(blobName);

        // Write to blob storage
        var blobWriterOptions = new BlobOpenWriteOptions
        {
            HttpHeaders = new BlobHttpHeaders()
            {
                ContentType = contentType
            }
        };
        using Stream blobStream = await blobClient.OpenWriteAsync(true, blobWriterOptions, cancellationToken);
        using BinaryWriter writer = new BinaryWriter(blobStream);

        // Write file header to blob, then write the rest of the image data
        writer.Write(fileHeader);
        readStream.CopyTo(writer.BaseStream);
        readStream.Close();
        writer.Flush();
        writer.Close();

        return new Blob(contentType, blobClient.Uri);

    }

    /// <inheritdoc/>
    public async Task DeleteBlobAsync(BlobContainerClient blobContainer, Guid name, CancellationToken cancellationToken)
    {
        var blobClient = blobContainer.GetBlobClient(name.ToString());
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the file extension to use based on the content type
    /// </summary>
    /// <param name="contentType"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown when content type is invalid.</exception>
    private string GetFileExtensionFromContentType(string contentType)
    {
        var extensions = new Dictionary<string, string>()
        {
            {"image/gif", ".gif"},
            {"image/png", ".png"},
            {"image/jpeg", ".jpeg"},
            {"video/webm", ".webm"},
        };
        string type = contentType.ToLowerInvariant();

        if (extensions.TryGetValue(type, out string? extension))
        {
            return extension;
        }

        throw new ArgumentException($"This content-type is not supported: {contentType}");
    }

    /// <summary>
    /// Reads the content header from a stream and determines the content type.
    /// If the header is not a supported format it throws an exception.
    /// </summary>
    /// <param name="stream">The stream of the form file.</param>
    /// <returns>The file header bytes and the content type for the stream.</returns>
    private (byte[], string) ReadAndValidateFileHeader(Stream stream)
    {
        byte[] fileHeader = new byte[4];
        stream.Read(fileHeader, 0, fileHeader.Length);

        var headers = new Dictionary<byte[], string>
        {
            {Encoding.ASCII.GetBytes("GIF"), "image/gif"},    // GIF
            {new byte[] { 137, 80, 78, 71 }, "image/png"},    // PNG
            {new byte[] { 255, 216, 255, 224 }, "image/jpeg"},// JPEG
            {new byte[] { 255, 216, 255, 225 }, "image/jpeg"},// JPEG CANON
            {new byte[] { 26, 69, 223, 163 }, "video/webm"},  // WEBM

            // Disabled formats
            // {Encoding.ASCII.GetBytes("BM"), "image/bmp"},     // BMP
            // {new byte[] { 73, 73, 42 }, "image/tiff"},        // TIFF
            // {new byte[] { 77, 77, 42 }, "image/tiff"},        // TIFF
        };

        KeyValuePair<byte[], string>? headerContentType = headers
            .FirstOrDefault(header => header.Key.SequenceEqual(fileHeader.Take(header.Key.Length)));

        if (headerContentType.HasValue == false)
        {
            throw new InklioDomainException(400, "Invalid file.");
        }

        return (fileHeader, headerContentType.Value.Value);
    }
}
