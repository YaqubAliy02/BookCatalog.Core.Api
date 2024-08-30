using Application.DTOs.BookDTO;
using Application.Repositories;
using AutoMapper;
using BookCatalog.Web.Core.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookCatalog.Web.Core.Controllers
{
    public class BookController : Controller
    { 
        private readonly IBookRepository _bookRepository;
        private IMapper _mapper;

        public BookController(IBookRepository bookRepository, 
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Book> books = await _bookRepository.GetAsync(x => true);
            IEnumerable<BookGetDto> bookResult = _mapper.Map<IEnumerable<BookGetDto>>(books);

            return View();
        }

        public IActionResult GetFileUpload()
        {
           return View(); 
        }
        public IActionResult  PostFileUpload(FileUploadModel file)
        {
            return View();
        }
    }
}
