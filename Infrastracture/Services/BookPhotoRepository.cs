using Application.Repositories;
using Infrastracture.External;

namespace Infrastracture.Services
{
    public class BookPhotoRepository : IBookPhotoRepository
    {
        private readonly IBlobStorage _blobStorage;

        public BookPhotoRepository(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<string> AddBookPhotoAsync(Stream fileStream, string fileName, string contentType) =>
              await _blobStorage.UploadBookPhotoAsync(fileStream, fileName, contentType);
    }
}
