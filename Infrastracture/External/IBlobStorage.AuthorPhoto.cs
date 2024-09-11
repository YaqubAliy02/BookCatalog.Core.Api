namespace Infrastracture.External
{
    public partial interface IBlobStorage
    {
        Task<string> UploadAuthorPhotoAsync(Stream fileStream, string fileName, string contentType);
    }
}