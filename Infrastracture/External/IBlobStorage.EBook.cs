namespace Infrastracture.External
{
    public partial interface IBlobStorage
    {
        Task<string> UploadEBookAsync(Stream fileStream, string fileName, string contentType);
        Task<Stream> DownloadEbookAsync(string fileName);
        /*        Task<Stream> StreamEBookAsync(string fileName);*/
    }
}
