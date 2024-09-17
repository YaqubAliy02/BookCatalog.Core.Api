using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace Infrastracture.External
{
    public partial class BlobStorage : IBlobStorage
    {

        private readonly string blobConnectionString;
        private readonly string photoContainerName;
        private readonly string ebookContainerName;

        private readonly HashSet<string> supportedPhotoExtensions =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { ".jpg", ".jpeg", ".png", ".gif" };

        private readonly HashSet<string> supportedPhotoContentTypes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };

        private readonly HashSet<string> supportedEbookExtensions =
               new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                   { ".pdf", ".epub", ".mobi" };

        private readonly HashSet<string> supportedEbookContentTypes =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "application/pdf",
            "application/epub+zip",
            "application/x-mobipocket-ebook"
        };

        public BlobStorage(IConfiguration configuration)
        {
            this.blobConnectionString = configuration["AzureBlobStorage:ConnectionString"];
            this.photoContainerName = configuration["AzureBlobStorage:PhotoContainerName"];
            this.ebookContainerName = configuration["AzureBlobStorage:EBookContainerName"];
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            string containerName;

            // Determine the appropriate container based on the file extension and content type
            if (supportedPhotoExtensions.Contains(extension) || supportedPhotoContentTypes.Contains(contentType))
            {
                containerName = photoContainerName;
            }
            else if (supportedEbookExtensions.Contains(extension) || supportedEbookContentTypes.Contains(contentType))
            {
                containerName = ebookContainerName;
            }
            else
            {
                throw new InvalidOperationException("Unsupported file extension or content type.");
            }

            // Set contentType to "application/pdf" if the file is a PDF
            if (extension == ".pdf")
            {
                contentType = "application/pdf";  // Force contentType to be PDF
            }

            var blobServiceClient = new BlobServiceClient(blobConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            // Upload the file with the correct ContentType
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });

            // Return the Blob URL
            return blobClient.Uri.ToString();
        }

    }
}
