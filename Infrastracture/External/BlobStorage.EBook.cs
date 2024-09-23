using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Entities;

namespace Infrastracture.External
{
    public partial class BlobStorage
    {
        public async Task<string> UploadEBookAsync(Stream fileStream, string fileName, string contentType) =>
            await UploadAsync(fileStream, fileName, contentType);

        public async Task<List<EBook>> SelectAllEbooksAsync()
        {
            var blobServiceClient = new BlobServiceClient(blobConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ebookContainerName);
            var blobItems = blobContainerClient.GetBlobsAsync();
            var allowedExtensions = new[] { ".pdf", ".epub", ".mobi" };

            var ebooks = new List<EBook>();

            await foreach (BlobItem blobItem in blobItems)
            {
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                var extension = Path.GetExtension(blobItem.Name);

                if (allowedExtensions.Contains(extension))
                {
                    var properties = await blobClient.GetPropertiesAsync();

                    ebooks.Add(new EBook
                    {
                        Id = Guid.NewGuid(),
                        FileName = blobItem.Name,
                        ContentType = properties.Value.ContentType,
                        Size = properties.Value.ContentLength,
                        BlobUri = blobClient.Uri.ToString(),
                        UploadedDate = properties.Value.CreatedOn.DateTime
                    });
                }
            }
            return ebooks;
        }

        public async Task<Stream> DownloadEbookAsync(string fileName)
        {
            var blobServiceClient = new BlobServiceClient(blobConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ebookContainerName);
            var blobClient = blobContainerClient.GetBlobClient(fileName);

            var exists = await blobClient.ExistsAsync();

            if (!exists)
            {
                throw new FileNotFoundException($"E-book with filename '{fileName}' does not exist.");
            }

            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content;
        }
    }
}
