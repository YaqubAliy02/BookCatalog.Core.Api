using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Web.Core.Filters
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedExtensionsAttribute( params string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;
            if(file is not null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(file.FileName)))
                {
                    return new ValidationResult($"You can only upload {string.Join(", ", _allowedExtensions)} format file");
                }
            }
            return ValidationResult.Success;
        }
    }
}
