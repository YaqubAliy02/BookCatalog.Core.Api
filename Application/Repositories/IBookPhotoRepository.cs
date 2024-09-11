namespace Application.Repositories
{
    public interface IBookPhotoRepository
    {
        Task<string> AddBookPhotoAsync(Stream fileStream, string fileName, string contentType);
    }
}
