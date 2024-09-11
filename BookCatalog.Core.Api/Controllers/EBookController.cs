﻿using Application.Repositories;
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
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file is null || !ValidateEBook(file))
            {
                throw new ArgumentNullException("Photo is not valid format");
            }
            using var stream = file.OpenReadStream();
            var blobUri = await _bookRepository.AddEBookPhotoAsync(stream, file.FileName, file.ContentType);

            var authorPhoto = new AuthorPhoto
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                BlobUri = blobUri,
                UploadedDate = DateTime.UtcNow,
            };

            return Ok(authorPhoto);
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadEbook(string fileName)
        {
            try
            {
                var ebookStream = await _bookRepository.DownloadEbookAsync(fileName);

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
            var extension = Path.GetExtension(file.FileName).ToLower();

            return file.Length > 0 && file.Length <= 50 * 1024 * 1024 && allowedExtensions.Contains(extension);
        }
    }
}