namespace Infrastracture.External
{
    public partial interface IBlobStorage
    {
        Task<string> UploadBookPhotoAsync(Stream fileStream, string fileName, string contentType);
    }
}
