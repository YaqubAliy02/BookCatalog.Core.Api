namespace Application.Repositories
{
    public interface IEBookRepository
    {
        Task<string> AddEBookPhotoAsync(Stream fileStream, string fileName, string contentType);
        Task<Stream> DownloadEbookAsync(string fileName);
    }
}
