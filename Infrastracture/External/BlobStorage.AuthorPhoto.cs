using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Entities;

namespace Infrastracture.External
{
    public partial class BlobStorage
    {
        public async Task<string> UploadAuthorPhotoAsync(Stream fileStream, string fileName, string contentType) =>
           await UploadAsync(fileStream, fileName, contentType);

        public async Task<List<AuthorPhoto>> SelectAllAuthorPhotosAsync()
        {
            var blobServiceClient = new BlobServiceClient(blobConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(photoContainerName);
            var blobItems = blobContainerClient.GetBlobsAsync();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var photos = new List<AuthorPhoto>();

            await foreach (BlobItem blobItem in blobItems)
            {
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                var extension = Path.GetExtension(blobItem.Name);

                if (allowedExtensions.Contains(extension))
                {
                    var properties = await blobClient.GetPropertiesAsync();

                    photos.Add(new AuthorPhoto
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
            return photos;
        }
    }
}
