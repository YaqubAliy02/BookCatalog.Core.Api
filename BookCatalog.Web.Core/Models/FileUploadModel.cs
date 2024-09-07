using BookCatalog.Web.Core.Filters;

namespace BookCatalog.Web.Core.Models
{
    public class FileUploadModel
    {
        [MaxFileSize(10)]
        [AllowedExtensions(".txt", ".jpg", "png")]
        public IFormFile UploadFile { get; set; }
    }
}
