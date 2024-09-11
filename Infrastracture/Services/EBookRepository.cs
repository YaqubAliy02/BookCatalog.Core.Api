using Application.Repositories;
using Infrastracture.External;

namespace Infrastracture.Services
{
    public class EBookRepository : IEBookRepository
    {
        private readonly IBlobStorage _blobStorage;

        public EBookRepository(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<string> AddEBookPhotoAsync(Stream fileStream, string fileName, string contentType) =>
              await _blobStorage.UploadEBookAsync(fileStream, fileName, contentType);

        public async Task<Stream> DownloadEbookAsync(string fileName) =>
            await _blobStorage.DownloadEbookAsync(fileName);
    }
}
