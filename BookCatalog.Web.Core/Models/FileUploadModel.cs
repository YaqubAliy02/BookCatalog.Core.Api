using System.ComponentModel.DataAnnotations;
using BookCatalog.Web.Core.Filters;

namespace BookCatalog.Web.Core.Models
{
    public class FileUploadModel
    {
        [Required]
        [MaxFileSize(1)]
        public IFormFile UploadFile { get; set; }
    }
}
