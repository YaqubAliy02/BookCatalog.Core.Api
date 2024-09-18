using Application.DTOs.BookDTO;
using Domain.Enums;

namespace BookCatalog.Core.Web.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BookGetDto>> GetBooks(Guid? authorId = null, BookCategories? category = null)
        {
            var queryParams = new List<string>();

            if (authorId.HasValue)
                queryParams.Add($"authorId={authorId.Value}");

            if (category.HasValue)
                queryParams.Add($"category={category.Value}");

            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;

            var response = await _httpClient.GetFromJsonAsync<IEnumerable<BookGetDto>>($"api/books{queryString}");

            return response ?? new List<BookGetDto>();
        }
    }

}
