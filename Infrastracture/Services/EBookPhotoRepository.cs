using Application.Repositories;
using Infrastracture.External;

namespace Infrastracture.Services
{
    public class EBookPhotoRepository : IEBookPhotoRepository
    {
        private readonly IBlobStorage _blobStorage;

        public EBookPhotoRepository(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<string> AddEBookPhotoAsync(Stream fileStream, string fileName, string contentType) =>
            await _blobStorage.UploadBookPhotoAsync(fileStream, fileName, contentType);
    }
}
