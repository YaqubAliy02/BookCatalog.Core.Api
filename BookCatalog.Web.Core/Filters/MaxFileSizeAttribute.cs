using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Web.Core.Filters
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize * 1024;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file  = value as IFormFile;
            if(file is not null)
            {
                if(file.Length > _maxFileSize)
                {
                    return new ValidationResult("File size must be lower than " + _maxFileSize + "kb");
                }
                return ValidationResult.Success;
            }
            return base.IsValid(value, validationContext);
        }
    }
}
