using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Inklio.Api.Domain;

namespace Inklio.Api.Infrastructure.Repositories;

public class BlobRepository : IBlobRepository
{
    private const string AskBlobBlobContainerName = "ask";
    private const string DeliveryBlobBlobContainerName = "delivery";
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
    public async Task<Uri> AddAskImageAsync(string contentType, IFormFile formFile, Guid name, CancellationToken cancellationToken)
    {
        // TODO: Figure out how to pass the content-type in the form file correctly
        contentType = "image/png";

        // Create blob
        string blobName = name.ToString() + this.GetFileExtensionFromContentType(contentType);
        var blobClient = this.askContainer.GetBlobClient(blobName);

        // Open streams for reading and writing the blob
        var blobWriterOptions = new BlobOpenWriteOptions
        {
            HttpHeaders = new BlobHttpHeaders()
            {
                ContentType = contentType
            }
        };
        using Stream blobStream = await blobClient.OpenWriteAsync(true, blobWriterOptions, cancellationToken);
        using BinaryWriter writer = new BinaryWriter(blobStream);
        using Stream readStream = formFile.OpenReadStream();

        // Read and validate the file header bytes, then write the file header bytes
        // to the blob, then write the rest of the image data to the blob.
        byte[] fileHeader = this.ReadAndValidateFileHeader(readStream);
        writer.Write(fileHeader);
        readStream.CopyTo(writer.BaseStream);
        readStream.Close();
        writer.Flush();
        writer.Close();

        return blobClient.Uri;
    }

    /// <inheritdoc/>
    public async Task DeleteAskImageAsync(Guid name, CancellationToken cancellationToken)
    {
        var blobClient = this.askContainer.GetBlobClient(name.ToString());
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    private string GetFileExtensionFromContentType(string contentType)
    {
        var extensions = new Dictionary<string, string>()
        {
            {"image/png", ".png"},
            {"image/gif", ".gif"},
            {"image/jpeg", ".jpeg"},
        };
        string type = contentType.ToLowerInvariant();

        if (extensions.TryGetValue(type, out string? extension))
        {
            return extension;
        }

        throw new InklioDomainException(400, "This filetype is not supported");
    }

    private static bool IsImage(byte[] fileBytes)
    {
        var headers = new List<byte[]>
        {
            Encoding.ASCII.GetBytes("BM"),      // BMP
            Encoding.ASCII.GetBytes("GIF"),     // GIF
            new byte[] { 137, 80, 78, 71 },     // PNG
            new byte[] { 73, 73, 42 },          // TIFF
            new byte[] { 77, 77, 42 },          // TIFF
            new byte[] { 255, 216, 255, 224 },  // JPEG
            new byte[] { 255, 216, 255, 225 }   // JPEG CANON
        };

        return headers.Any(x => x.SequenceEqual(fileBytes.Take(x.Length)));
    }

    private byte[] ReadAndValidateFileHeader(Stream stream)
    {
        byte[] buffer = new byte[4];
        stream.Read(buffer, 0, buffer.Length);
        if (IsImage(buffer) == false)
        {
            throw new InklioDomainException(400, "Invalid file");
        }
        return buffer;
    }
}
