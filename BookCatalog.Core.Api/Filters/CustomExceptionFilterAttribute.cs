using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookCatalog.Core.Api.Filters
{
    public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var result = new ObjectResult("An error occured. This is Custom Exception Filter error handler")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            context.Result = result;
        }
    }
}
