using Application.Repositories;
using Infrastracture.External;

namespace Infrastracture.Services
{
    public class AuthorPhotoRepository : IAuthorPhotoRepository
    {
        private readonly IBlobStorage _blobStorage;

        public AuthorPhotoRepository(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<string> AddAuthorPhotoAsync(Stream fileStream, string fileName, string contentType) =>
              await _blobStorage.UploadAuthorPhotoAsync(fileStream, fileName, contentType);
    }
}
