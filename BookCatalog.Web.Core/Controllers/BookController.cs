using Application.DTOs.BookDTO;
using Application.Repositories;
using AutoMapper;
using BookCatalog.Web.Core.Filters;
using BookCatalog.Web.Core.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Web.Core.Controllers
{
    [MaxFileSize(1)]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(IBookRepository bookRepository,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Book> books = await _bookRepository.GetAsync(x => true);
            IEnumerable<BookGetDto> bookResult = _mapper.Map<IEnumerable<BookGetDto>>(books);

            return View();
        }

        /* [HttpPost("[action]")]
         public async Task<IActionResult> UpdateBook([FromForm] UpdateBookCommand)
 */
        [HttpGet("[action]")]
        public IActionResult SearchBook(string text)
        {
            return Ok(_bookRepository.SearchBook(text));
        }

        public IActionResult GetFileUpload()
        {
            ViewBag.Files = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "images"));
            return View();
        }

        public IActionResult PostFileUpload(FileUploadModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UploadFile is not null)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string fileName = Guid.NewGuid().ToString() + "_" + model.UploadFile.FileName
                        .Replace(@"\", "_")
                        .Replace(@"/", "_");

                    string filePath = Path.Combine(path, fileName);
                    model.UploadFile.CopyToAsync(new FileStream(filePath, FileMode.Create));
                }

                return RedirectToAction("GetFileUpload");
            }
            ViewBag.Files = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "images"));

            return View("GetFileUpload");
        }

    }
}
