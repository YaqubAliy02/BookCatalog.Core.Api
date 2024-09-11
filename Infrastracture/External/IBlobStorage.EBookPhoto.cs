namespace Infrastracture.External
{
    public partial interface IBlobStorage
    {
        Task<string> UploadEBookPhotoAsync(Stream fileStream, string fileName, string contentType);
    }
}
