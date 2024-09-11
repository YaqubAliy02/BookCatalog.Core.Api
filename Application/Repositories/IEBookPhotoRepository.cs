namespace Application.Repositories
{
    public interface IEBookPhotoRepository
    {
        Task<string> AddEBookPhotoAsync(Stream fileStream, string fileName, string contentType);
    }
}
