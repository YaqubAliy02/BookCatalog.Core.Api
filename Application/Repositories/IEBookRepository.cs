namespace Application.Repositories
{
    public interface IEBookRepository
    {
        Task<string> AddEBookPhotoAsync(Stream fileStream, string fileName, string contentType);
        Task<Stream> DownloadEBookAsync(string fileName);
        //Task<Stream> StreamEBookAsync(string fileName);
    }
}
