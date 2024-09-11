using Domain.Entities;

namespace Application.Repositories
{
    public interface IAuthorPhotoRepository
    {
        Task<string> AddAuthorPhotoAsync(Stream fileStream, string fileName, string contentType);
    }
}
