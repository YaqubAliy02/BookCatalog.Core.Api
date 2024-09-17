using Application.Repositories;
using Azure.Storage.Blobs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class EBookController : ApiControllerBase
    {
        private readonly IEBookRepository _bookRepository;

        public EBookController(IEBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadEBook(IFormFile file)
        {
            if (file is null || !ValidateEBook(file))
            {
                throw new ArgumentNullException("Photo is not valid format");
            }
            using var stream = file.OpenReadStream();
            var blobUri = await _bookRepository.AddEBookPhotoAsync(stream, file.FileName, file.ContentType);

            var eBook = new EBook
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                BlobUri = blobUri,
                UploadedDate = DateTime.UtcNow,
            };

            return Ok(eBook);
        }

        [HttpGet("stream/{fileName}")]
        public async Task<IActionResult> StreamEbook(string fileName)
        {
            try
            {
                // Download the e-book file from Blob Storage using the file name
                var ebookStream = await _bookRepository.DownloadEBookAsync(fileName);

                if (ebookStream == null)
                {
                    return NotFound("E-book file not found.");
                }

                // Define the content type based on the file extension
                var extension = Path.GetExtension(fileName).ToLower();
                string contentType = extension switch
                {
                    ".pdf" => "application/pdf",
                    ".epub" => "application/epub+zip",
                    ".mobi" => "application/x-mobipocket-ebook",
                    _ => "application/octet-stream"
                };

                // Set Content-Disposition header to "inline" to open in browser
                Response.Headers.Add("Content-Disposition", $"inline; filename={fileName}");

                // Return the e-book file as a FileStreamResult for streaming
                return new FileStreamResult(ebookStream, contentType)
                {
                    FileDownloadName = fileName
                };
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadEbook(string fileName)
        {
            try
            {
                var ebookStream = await _bookRepository.DownloadEBookAsync(fileName);

                if (ebookStream == null)
                {
                    return NotFound();
                }

                // Define the content type based on file extension
                var extension = Path.GetExtension(fileName).ToLower();
                string contentType = extension switch
                {
                    ".pdf" => "application/pdf",
                    ".epub" => "application/epub+zip",
                    ".mobi" => "application/x-mobipocket-ebook",
                    _ => "application/octet-stream"
                };

                return File(ebookStream, contentType, fileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool ValidateEBook(IFormFile file)
        {
            var allowedExtensions = new[] { ".pdf", ".epub", ".mobi" };

            var extension = Path.GetExtension(file.FileName)?.ToLower();

            if (file is null || file.Length is 0)
            {
                Console.WriteLine("File cannot be null or empty.");
                return false;
            }
            if (file.Length > 50 * 1024 * 1024) // 50 MB in bytes
            {
                Console.WriteLine("The file size exceeds the 50 MB limit.");
                return false;
            }
            if (!allowedExtensions.Contains(extension))
            {
                Console.WriteLine($"The file must have one of the allowed extensions: {string.Join(", ", allowedExtensions)}.");
                return false;
            }

            Console.WriteLine("File is valid.");
            return true;
        }

    }
}
